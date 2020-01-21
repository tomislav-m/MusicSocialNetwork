using AutoMapper;
using CatalogService.Models;
using CatalogService.Services;
using Common.MessageContracts.Catalog.Commands;
using Common.MessageContracts.Catalog.Events;
using MassTransit;
using System.Threading.Tasks;

namespace CatalogService.Consumers
{
    public class CustomTagConsumer : IConsumer<AddToCollection>
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

        public async Task Consume(ConsumeContext<AddToCollection> context)
        {
            var message = context.Message;

            try
            {
                var albumTag = new UserAlbum { AlbumId = message.AlbumId, UserId = message.UserId };
                await _service.AddToCollection(albumTag);

                var @event = _mapper.Map<AddToCollection, AlbumAddedToCollection>(message);
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
