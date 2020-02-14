using Common.MessageContracts.Recommender.Commands;
using Common.MessageContracts.Recommender.Events;
using MassTransit;
using RecommenderService.Services;
using System;
using System.Threading.Tasks;

namespace RecommenderService.Consumers
{
    public class RecommendationConsumer : IConsumer<GetRecommendations>
    {
        private readonly IRecommendationService _service;
        private readonly EventStoreService _eventStoreService;

        public RecommendationConsumer(IRecommendationService service, EventStoreService eventStoreService)
        {
            _service = service;
            _eventStoreService = eventStoreService;
        }

        public async Task Consume(ConsumeContext<GetRecommendations> context)
        {
            var userId = context.Message.UserId;

            try
            {
                var userAlbumIds = _service.GetUserAlbumIds(userId);
                userAlbumIds = await _service.FilterAlbumsByStylesAndGenres(userAlbumIds);
                userAlbumIds = await _service.GetPopularAlbums(userAlbumIds);

                await context.RespondAsync(new Recommendations { AlbumIds = userAlbumIds });
            }
            catch (Exception exc)
            {
                await context.RespondAsync(new Recommendations { Exception = exc });
            }
        }
    }
}
