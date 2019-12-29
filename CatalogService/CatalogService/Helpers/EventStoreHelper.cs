using AutoMapper;
using CatalogService.MessageContracts;
using CatalogService.Models;
using CatalogService.Services;
using Common.MessageContracts;
using Common.MessageContracts.Catalog.Events;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Helpers
{
    public class EventStoreHelper
    {
        private static IEventStoreConnection connection;
        private readonly IMapper _mapper;
        private readonly IRatingService _ratingService;
        private readonly ITagService _TagService;

        public EventStoreHelper(
            IRatingService ratingService,
            ITagService TagService,
            IMapper mapper)
        {
            _ratingService = ratingService;
            _TagService = TagService;
            _mapper = mapper;
        }

        public static async void Init()
        {
            connection = EventStoreConnection.Create(new Uri("tcp://admin:changeit@localhost:1113"));
            await connection.ConnectAsync();
        }

        public static async void AddEventToStream(IEvent @event)
        {
            var myEvent = new EventData(Guid.NewGuid(), @event.Type, true,
                            Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event)),
                            Encoding.UTF8.GetBytes(@event.Type));

            await connection.AppendToStreamAsync("catalog-stream", ExpectedVersion.Any, myEvent);
        }

        public async Task<ResolvedEvent[]> ReadFromStream()
        {
            var eventsSlice = await connection.ReadAllEventsForwardAsync(Position.Start, int.MaxValue, false);
            return eventsSlice.Events;
        }

        public async void RecreateDb()
        {
            var events = await ReadFromStream();
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
                        await _TagService.AddToTag(_mapper.Map<AlbumAddedToTag, AlbumTag>(albumAdded));
                        break;
                    case MessageContract.TagAdded:
                        var TagAddedEvent = JsonConvert.DeserializeObject<CustomTagAdded>(
                            Encoding.UTF8.GetString(@event.Event.Data));
                        await _TagService.CreateTag(_mapper.Map<CustomTagAdded, Tag>(TagAddedEvent));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
