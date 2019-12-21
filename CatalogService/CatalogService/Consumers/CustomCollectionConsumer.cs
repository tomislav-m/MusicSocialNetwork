using AutoMapper;
using CatalogService.Helpers;
using CatalogService.MessageContracts;
using CatalogService.Models;
using CatalogService.Services;
using MassTransit;
using System.Threading.Tasks;

namespace CatalogService.Consumers
{
    public class CustomTagConsumer : IConsumer<AddCustomTag>, IConsumer<AddToTag>
    {
        private readonly ITagService _service;
        private readonly IMapper _mapper;

        public CustomTagConsumer(ITagService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<AddCustomTag> context)
        {
            var message = context.Message;

            try
            {
                var Tag = await _service.CreateTag(new Tag { Name = message.Name, UserId = message.UserId });

                var @event = _mapper.Map<Tag, CustomTagAdded>(Tag);
                await context.RespondAsync(@event);
                //EventStoreHelper.AddEventToStream(@event);
            }
            catch
            {
                await context.RespondAsync(null);
            }
        }

        public async Task Consume(ConsumeContext<AddToTag> context)
        {
            var message = context.Message;

            try
            {
                var albumTag = new AlbumTag { AlbumId = message.AlbumId, AlbumTagId = message.TagId };
                await _service.AddToTag(albumTag);

                var @event = _mapper.Map<AddToTag, AlbumAddedToTag>(message);
                await context.RespondAsync(@event);
                //EventStoreHelper.AddEventToStream(@event);
            }
            catch
            {
                await context.RespondAsync(null);
            }
        }
    }
}
