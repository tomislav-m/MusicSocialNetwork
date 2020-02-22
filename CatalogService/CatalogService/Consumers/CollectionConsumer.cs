using AutoMapper;
using CatalogService.Models;
using CatalogService.Services;
using Common.MessageContracts.Catalog.Commands;
using Common.MessageContracts.Catalog.Events;
using Common.Services;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Consumers
{
    public class CollectionConsumer : IConsumer<AddToCollection>, IConsumer<GetPopularAlbums>, IConsumer<GetCollection>
    {
        private readonly ICollectionService _service;
        private readonly IMapper _mapper;
        private readonly EventStoreService _eventStoreService;

        public CollectionConsumer(ICollectionService service, IMapper mapper, EventStoreService eventStoreService)
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
            catch (Exception ex)
            {
                await context.RespondAsync(new AlbumAddedToCollection { Exception = ex });
            }
        }

        public async Task Consume(ConsumeContext<GetPopularAlbums> context)
        {
            try
            {
                (IEnumerable<int> dayIds, IEnumerable<int> weekIds, IEnumerable<int> monthIds) = await _eventStoreService.GetPopularAlbums();
                var @event = new PopularAlbums { TodayAlbums = dayIds, WeekAlbums = weekIds, MonthAlbums = monthIds };
                await context.RespondAsync(@event);
            }
            catch
            {
                await context.RespondAsync(null);
            }
        }

        public async Task Consume(ConsumeContext<GetCollection> context)
        {
            var message = context.Message;

            try
            {
                var collection = await _service.GetCollection(message.Id);
                var @event = new Collection { AlbumIds = collection.Select(x => x.AlbumId) };
                await context.RespondAsync(@event);
            }
            catch (Exception exc)
            {
                await context.RespondAsync(new Collection { Exception = exc });
            }
        }
    }
}
