using Common.MessageContracts.User.Commands;
using Common.MessageContracts.User.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IRequestClient<SignInUser, UserSignedIn> _requestClient;
        private readonly IRequestClient<CreateUser, UserCreated> _registerRequestClient;

        public UsersController(
            IRequestClient<SignInUser, UserSignedIn> requestClient,
            IRequestClient<CreateUser, UserCreated> registerRequestClient
            )
        {
            _requestClient = requestClient;
            _registerRequestClient = registerRequestClient;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(SignInUser credentials)
        {
            try
            {
                UserSignedIn result = await _requestClient.Request(credentials);

                if (string.IsNullOrEmpty(result.Token))
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

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUser credentials)
        {
            try
            {
                UserCreated result = await _registerRequestClient.Request(credentials);

                if (result.Exception != null)
                {
                    return CreatedAtRoute(nameof(Register), result);
                }
                else
                {
                    throw result.Exception;
                }
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }
    }
}
