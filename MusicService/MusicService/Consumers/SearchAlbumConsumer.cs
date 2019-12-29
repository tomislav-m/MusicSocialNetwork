using AutoMapper;
using Common.MessageContracts.Music.Commands;
using Common.MessageContracts.Music.Events;
using MassTransit;
using MusicService.Service.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicService.Service.Consumers
{
    public class SearchAlbumConsumer : IConsumer<SearchAlbum>
    {
        private readonly IAlbumService _service;
        private readonly IMapper _mapper;

        public SearchAlbumConsumer(IAlbumService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<SearchAlbum> context)
        {
            var message = context.Message;

            var albums = await _service.Search(message.SearchTerm, message.Page, message.Size);

            await context.RespondAsync(_mapper.Map<IEnumerable<DomainModel.Album>, AlbumFound[]>(albums));
        }
    }
}
