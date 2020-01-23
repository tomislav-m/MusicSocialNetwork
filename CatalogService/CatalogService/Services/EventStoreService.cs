using AutoMapper;
using CatalogService.MessageContracts;
using CatalogService.Models;
using Common.MessageContracts.Catalog.Events;
using Common.Services;
using Newtonsoft.Json;
using System.Text;

namespace CatalogService.Services
{
    public class EventStoreService : EventStoreServiceBase
    {
        private readonly IMapper _mapper;
        private readonly IRatingService _ratingService;
        private readonly ICollectionService _collectionService;

        public EventStoreService(
            IRatingService ratingService,
            ICollectionService collectionService,
            IMapper mapper)
        {
            _ratingService = ratingService;
            _collectionService = collectionService;
            _mapper = mapper;
            Init();
        }

        public override async void RecreateDbAsync()
        {
            var events = await ReadFromStream("catalog-stream");
            foreach (var @event in events)
            {
                var type = @event.Event.EventType;

                switch (type)
                {
                    case MessageContract.AlbumRated:
                        var albumRatedEvent = JsonConvert.DeserializeObject<AlbumRated>(
                            Encoding.UTF8.GetString(@event.Event.Data));
                        await _ratingService.RateAlbum(_mapper.Map<AlbumRated, AlbumRating>(albumRatedEvent));
                        break;
                    case MessageContract.AlbumAdded:
                        var albumAdded = JsonConvert.DeserializeObject<AlbumAddedToCollection>(
                            Encoding.UTF8.GetString(@event.Event.Data));
                        await _collectionService.AddToCollection(_mapper.Map<AlbumAddedToCollection, UserAlbum>(albumAdded));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
