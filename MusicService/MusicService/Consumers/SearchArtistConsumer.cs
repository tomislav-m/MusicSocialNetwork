using AutoMapper;
using Common.MessageContracts.Music.Commands;
using Common.MessageContracts.Music.Events;
using MassTransit;
using MusicService.Service.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Artist = MusicService.DomainModel.Artist;

namespace MusicService.Service.Consumers
{
    public class SearchArtistConsumer : IConsumer<SearchArtist>
    {
        private readonly IArtistService _service;
        private readonly IMapper _mapper;

        public SearchArtistConsumer(IArtistService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<SearchArtist> context)
        {
            var message = context.Message;

            var artists = await _service.Search(message.SearchTerm, message.Page, message.Size);

            await context.RespondAsync(_mapper.Map<IEnumerable<Artist>, ArtistFound[]>(artists)); 
        }
    }
}
