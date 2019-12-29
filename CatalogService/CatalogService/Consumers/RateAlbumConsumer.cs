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

        public RateAlbumConsumer(IRatingService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<RateAlbum> context)
        {
            var message = context.Message;

            try
            {
                var rating = await _service.RateAlbum(_mapper.Map<RateAlbum, AlbumRating>(message));

                var @event = _mapper.Map<AlbumRating, AlbumRated>(rating);
                await context.RespondAsync(@event);
                //EventStoreHelper.AddEventToStream(@event);
            }
            catch (Exception exc)
            {
                await context.RespondAsync(null);
            }
        }
    }
}
