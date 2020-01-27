using AutoMapper;
using CatalogService.Models;
using CatalogService.Services;
using Common.MessageContracts.Catalog.Commands;
using Common.MessageContracts.Catalog.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlbumRated = Common.MessageContracts.Catalog.Events.AlbumRated;

namespace CatalogService.Consumers
{
    public class RateAlbumConsumer : IConsumer<RateAlbum>, IConsumer<GetAverageRating>, IConsumer<GetRatedAlbums>
    {
        private readonly IRatingService _service;
        private readonly IMapper _mapper;
        private readonly EventStoreService _eventStoreService;

        public RateAlbumConsumer(IRatingService service, IMapper mapper, EventStoreService eventStoreService)
        {
            _service = service;
            _mapper = mapper;
            _eventStoreService = eventStoreService;
        }

        public async Task Consume(ConsumeContext<RateAlbum> context)
        {
            var message = context.Message;

            try
            {
                var rating = await _service.RateAlbum(_mapper.Map<RateAlbum, Models.AlbumRating>(message));

                var @event = _mapper.Map<Models.AlbumRating, AlbumRated>(rating);
                await context.RespondAsync(@event);
                _eventStoreService.AddEventToStream(@event, "catalog-stream");
            }
            catch (Exception exc)
            {
                await context.RespondAsync(new AlbumRated { Exception = exc });
            }
        }

        public async Task Consume(ConsumeContext<GetAverageRating> context)
        {
            var message = context.Message;

            try
            {
                var avgRating = await _service.GetAlbumAverageRating(message.AlbumId);
                await context.RespondAsync(new AlbumAverageRating { AverageRating = avgRating.Item1, RatingCount = avgRating.Item2 });
            }
            catch
            {
            }
        }

        public async Task Consume(ConsumeContext<GetRatedAlbums> context)
        {
            var message = context.Message;

            try
            {
                var userRatings = await _service.GetUserRatings(message.Id);
                await context.RespondAsync(_mapper.Map<IEnumerable<AlbumRating>, AlbumRated[]>(userRatings));
            }
            catch
            {
            }
        }
    }
}
