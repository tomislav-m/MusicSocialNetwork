using Common.Services;
using Newtonsoft.Json;
using RecommenderService.Models;
using System.Text;
using System.Threading.Tasks;

namespace RecommenderService.Services
{
    public class EventStoreService : EventStoreServiceBase
    {
        private readonly IRecommendationService _service;

        public EventStoreService(IRecommendationService service)
        {
            _service = service;

            Init().Wait();
        }

        public override async Task RecreateDbAsync()
        {
            var events = await ReadFromStream("catalog-stream");

            foreach (var @event in events)
            {
                var catalogModel = JsonConvert.DeserializeObject<CatalogModel>(
                    Encoding.UTF8.GetString(@event.Event.Data));

                await _service.Catalogize(catalogModel);
            }
        }
    }
}
