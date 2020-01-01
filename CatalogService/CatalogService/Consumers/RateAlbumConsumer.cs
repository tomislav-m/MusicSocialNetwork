using AutoMapper;
using CatalogService.Models;
using CatalogService.Services;
using Common.MessageContracts.Catalog.Commands;
using Common.MessageContracts.Catalog.Events;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace CatalogService.Consumers
{
    public class RateAlbumConsumer : IConsumer<RateAlbum>
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
                var rating = await _service.RateAlbum(_mapper.Map<RateAlbum, AlbumRating>(message));

                var @event = _mapper.Map<AlbumRating, AlbumRated>(rating);
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
