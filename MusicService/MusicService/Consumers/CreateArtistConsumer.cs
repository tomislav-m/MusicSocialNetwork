using AutoMapper;
using MassTransit;
using MusicService.MessageContracts;
using MusicService.Service.Services;
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
            catch
            {
                await context.RespondAsync(null);
            }
        }
    }
}
