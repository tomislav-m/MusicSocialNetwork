using AutoMapper;
using Common.MessageContracts.User.Commands;
using Common.MessageContracts.User.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;
using UserService.Services;

namespace UserService.Consumers
{
    public class CommentsConsumer : IConsumer<AddComment>, IConsumer<GetComments>
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        private readonly EventStoreService _eventStoreService;

        public CommentsConsumer(IUserService service, IMapper mapper, EventStoreService eventStoreService)
        {
            _service = service;
            _mapper = mapper;
            _eventStoreService = eventStoreService;
        }

        public async Task Consume(ConsumeContext<AddComment> context)
        {
            var message = context.Message;
            var comment = await _service.AddComment(new Comment { Author = message.Author, PageType = message.PageType, Text = message.Text, ParentId = message.ParentId });
            var commentAdded = _mapper.Map<Comment, CommentEvent>(comment);
            var author = _service.GetById(message.Author);
            commentAdded.Author = author.Username;

            try
            {
                await context.RespondAsync(commentAdded);
                _eventStoreService.AddEventToStream(commentAdded, "user-stream");
            }
            catch (Exception exc)
            {
                commentAdded.Exception = exc;
                await context.RespondAsync(commentAdded);
            }
        }

        public async Task Consume(ConsumeContext<GetComments> context)
        {
            var message = context.Message;
            var comments = _service.GetComments(message.PageType, message.ParentId);

            var commentList = new List<CommentEvent>();

            foreach (var comment in comments)
            {
                var commentEvent = _mapper.Map<Comment, CommentEvent>(comment);
                var author = _service.GetById(comment.Author);
                commentEvent.Author = author.Username;

                commentList.Add(commentEvent);
            }

            try
            {
                await context.RespondAsync(commentList.ToArray());
            }
            catch (Exception exc)
            {
                await context.RespondAsync(exc);
            }
        }
    }
}
