using AutoMapper;
using Common.MessageContracts.Music.Commands;
using Common.MessageContracts.Music.Events;
using MassTransit;
using MusicService.Service.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicService.Service.Consumers
{
    public class GetArtistConsumer : IConsumer<GetArtist>, IConsumer<GetArtistNamesByIds>
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
                await context.RespondAsync(new Artist { Exception = exc });
            }
        }

        public async Task Consume(ConsumeContext<GetArtistNamesByIds> context)
        {
            var message = context.Message;

            try
            {
                var list = new List<ArtistSimple>();

                foreach(var id in message.Ids)
                {
                    var artist = await _service.GetById(id);
                    list.Add(new ArtistSimple { Id = id, Name = artist.Name });
                }

                await context.RespondAsync(list.ToArray());
            }
            catch (Exception exc)
            {
                await context.RespondAsync(new Artist { Exception = exc });
            }
        }
    }
}
