using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using UserService.MessageContracts;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IRequestClient<SignInUser, UserSignedIn> _requestClient;

        public UsersController(IRequestClient<SignInUser, UserSignedIn> requestClient)
        {
            _requestClient = requestClient;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate(SignInUser credentials)
        {
            try
            {
                UserSignedIn result = await _requestClient.Request(new { Username = credentials.Username, Password = credentials.Password });

                if (string.IsNullOrEmpty(result.Token))
                {
                    return NotFound();
                }

                return Accepted(result.Token); 
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }
    }
}
