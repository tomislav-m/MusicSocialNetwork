using AutoMapper;
using Common.MessageContracts.Music.Commands;
using Common.MessageContracts.Music.Events;
using MassTransit;
using MusicService.Service.Services;
using System;
using System.Threading.Tasks;
using Artist = MusicService.DomainModel.Artist;

namespace MusicService.Service.Consumers
{
    public class CreateArtistConsumer : IConsumer<CreateArtist>
    {
        private readonly IArtistService _service;
        private readonly IMapper _mapper;

        public CreateArtistConsumer(IArtistService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<CreateArtist> context)
        {
            var message = context.Message;

            try
            {
                var artist = await _service.Create(_mapper.Map<CreateArtist, Artist>(message));

                await context.RespondAsync(_mapper.Map<Artist, ArtistCreated>(artist));
            }
            catch (Exception exc)
            {
                await context.RespondAsync(new ArtistCreated { Exception = exc });
            }
        }
    }
}
