using AutoMapper;
using Common.MessageContracts.Music.Commands;
using Common.MessageContracts.Music.Events;
using MassTransit;
using MusicService.Service.Services;
using System.Threading.Tasks;

namespace MusicService.Service.Consumers
{
    public class GetAlbumConsumer : IConsumer<GetAlbum>
    {
        private readonly IAlbumService _service;
        private readonly IMapper _mapper;

        public GetAlbumConsumer(IAlbumService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<GetAlbum> context)
        {
            var message = context.Message;

            var album = await _service.GetById(message.Id);

            await context.RespondAsync(_mapper.Map<DomainModel.Album, Album>(album));
        }
    }
}
