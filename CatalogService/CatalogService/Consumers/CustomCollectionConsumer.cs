using AutoMapper;
using CatalogService.Models;
using CatalogService.Services;
using Common.MessageContracts.Catalog.Commands;
using Common.MessageContracts.Catalog.Events;
using MassTransit;
using System.Threading.Tasks;

namespace CatalogService.Consumers
{
    public class CustomTagConsumer : IConsumer<AddCustomTag>, IConsumer<AddToTag>
    {
        private readonly ITagService _service;
        private readonly IMapper _mapper;
        private readonly EventStoreService _eventStoreService;

        public CustomTagConsumer(ITagService service, IMapper mapper, EventStoreService eventStoreService)
        {
            _service = service;
            _mapper = mapper;
            _eventStoreService = eventStoreService;
        }

        public async Task Consume(ConsumeContext<AddCustomTag> context)
        {
            var message = context.Message;

            try
            {
                var Tag = await _service.CreateTag(new Tag { Name = message.Name, UserId = message.UserId });

                var @event = _mapper.Map<Tag, CustomTagAdded>(Tag);
                await context.RespondAsync(@event);
                _eventStoreService.AddEventToStream(@event, "catalog-stream");
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
                _eventStoreService.AddEventToStream(@event, "catalog-stream");
            }
            catch
            {
                await context.RespondAsync(null);
            }
        }
    }
}
