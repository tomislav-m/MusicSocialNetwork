using AutoMapper;
using MassTransit;
using MusicService.MessageContracts;
using MusicService.Service.Services;
using System;
using System.Threading.Tasks;

namespace MusicService.Service.Consumers
{
    public class GetArtistConsumer : IConsumer<GetArtist>
    {
        private readonly IArtistService _service;
        private readonly IMapper _mapper;

        public GetArtistConsumer(IArtistService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetArtist> context)
        {
            var message = context.Message;

            try
            {
                var artist = await _service.GetById(message.Id);

                await context.RespondAsync(_mapper.Map<DomainModel.Artist, Artist>(artist));
            }
            catch (Exception exc)
            {
                await context.RespondAsync(null);
            }
        }
    }
}
