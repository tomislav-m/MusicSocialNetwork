using AutoMapper;
using CatalogService.MessageContracts;
using CatalogService.Models;
using Common.MessageContracts.Catalog.Events;
using Common.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Init().Wait();
        }

        public override async Task RecreateDbAsync()
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

        public async Task<(IEnumerable<int>, IEnumerable<int>, IEnumerable<int>)> GetPopularAlbums()
        {
            var events = await ReadFromStream("catalog-stream");

            var dayIds = new List<int>();
            var weekIds = new List<int>();
            var monthIds = new List<int>();
            foreach (var @event in events)
            {
                var created = @event.Event.Created;
                if (created.AddDays(30) < DateTime.Now)
                {
                    break;
                }
                switch (@event.Event.EventType)
                {
                    case MessageContract.AlbumRated:
                        var albumRatedEvent = JsonConvert.DeserializeObject<AlbumRated>(
                            Encoding.UTF8.GetString(@event.Event.Data));
                        if (created.AddDays(1) > DateTime.Now)
                        {
                            dayIds.Add(albumRatedEvent.AlbumId);
                            weekIds.Add(albumRatedEvent.AlbumId);
                            monthIds.Add(albumRatedEvent.AlbumId);
                        }
                        else if (created.AddDays(7) > DateTime.Now)
                        {
                            weekIds.Add(albumRatedEvent.AlbumId);
                            monthIds.Add(albumRatedEvent.AlbumId);
                        }
                        else if (created.AddDays(30) > DateTime.Now)
                        {
                            monthIds.Add(albumRatedEvent.AlbumId);
                        }
                        break;
                    case MessageContract.AlbumAdded:
                        var albumAdded = JsonConvert.DeserializeObject<AlbumAddedToCollection>(
                            Encoding.UTF8.GetString(@event.Event.Data));
                        if (created.AddDays(1) > DateTime.Now)
                        {
                            dayIds.Add(albumAdded.AlbumId);
                            weekIds.Add(albumAdded.AlbumId);
                            monthIds.Add(albumAdded.AlbumId);
                        }
                        else if (created.AddDays(7) > DateTime.Now)
                        {
                            weekIds.Add(albumAdded.AlbumId);
                            monthIds.Add(albumAdded.AlbumId);
                        }
                        else if (created.AddDays(30) > DateTime.Now)
                        {
                            monthIds.Add(albumAdded.AlbumId);
                        }
                        break;
                    default:
                        break;
                }
            }
            var dayDict = dayIds.GroupBy(x => x)
                .ToDictionary(x => x.Key, y => y.Count())
                .OrderByDescending(x => x.Value).Take(5)
                .Select(x => x.Key);
            var weekDict = weekIds.GroupBy(x => x)
                .ToDictionary(x => x.Key, y => y.Count())
                .OrderByDescending(x => x.Value).Take(5)
                .Select(x => x.Key);
            var monthDict = monthIds.GroupBy(x => x)
                .ToDictionary(x => x.Key, y => y.Count())
                .OrderByDescending(x => x.Value).Take(5)
                .Select(x => x.Key);

            return (dayDict, weekDict, monthDict);
        }
    }
}
