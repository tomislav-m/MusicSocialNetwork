using Common.MessageContracts.Recommender.Commands;
using Common.MessageContracts.Recommender.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        private readonly IRequestClient<GetRecommendations, Recommendations> _requestClient;

        public RecommendationsController(IRequestClient<GetRecommendations, Recommendations> requestClient)
        {
            _requestClient = requestClient;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<Recommendations>> GetRecommendation(int userId)
        {
            var result = await _requestClient.Request(new GetRecommendations { UserId = userId });

            try
            {
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }
    }
}
