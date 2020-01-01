using AutoMapper;
using CatalogService.MessageContracts;
using CatalogService.Models;
using Common.MessageContracts.Catalog.Events;
using Common.Services;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System.Text;

namespace CatalogService.Services
{
    public class EventStoreService : EventStoreServiceBase
    {
        private readonly IMapper _mapper;
        private readonly IRatingService _ratingService;
        private readonly ITagService _tagService;

        public EventStoreService(
            IRatingService ratingService,
            ITagService tagService,
            IMapper mapper)
        {
            _ratingService = ratingService;
            _tagService = tagService;
            _mapper = mapper;
            Init();
        }

        public override async void RecreateDb()
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
                        var albumAdded = JsonConvert.DeserializeObject<AlbumAddedToTag>(
                            Encoding.UTF8.GetString(@event.Event.Data));
                        await _tagService.AddToTag(_mapper.Map<AlbumAddedToTag, AlbumTag>(albumAdded));
                        break;
                    case MessageContract.TagAdded:
                        var TagAddedEvent = JsonConvert.DeserializeObject<CustomTagAdded>(
                            Encoding.UTF8.GetString(@event.Event.Data));
                        await _tagService.CreateTag(_mapper.Map<CustomTagAdded, Tag>(TagAddedEvent));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
