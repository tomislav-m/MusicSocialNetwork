using Common.MessageContracts.Catalog.Commands;
using Common.MessageContracts.Catalog.Events;
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
    public class UsersController : ControllerBase
    {
        private readonly IRequestClient<SignInUser, UserSignedIn> _requestClient;
        private readonly IRequestClient<CreateUser, UserCreated> _registerRequestClient;
        private readonly IRequestClient<AddComment, CommentEvent> _addCommentRequestClient;
        private readonly IRequestClient<GetComments, CommentEvent[]> _getCommentRequestClient;

        public UsersController(
            IRequestClient<SignInUser, UserSignedIn> requestClient,
            IRequestClient<CreateUser, UserCreated> registerRequestClient,
            IRequestClient<AddComment, CommentEvent> addCommentRequestClient,
            IRequestClient<GetComments, CommentEvent[]> getCommentRequestClient
            )
        {
            _requestClient = requestClient;
            _registerRequestClient = registerRequestClient;
            _addCommentRequestClient = addCommentRequestClient;
            _getCommentRequestClient = getCommentRequestClient;
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
                var result = await _registerRequestClient.Request(credentials);

                if (result.Exception == null)
                {
                    return Ok(result);
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

        [HttpPost("comment")]
        public async Task<IActionResult> AddComment(AddComment comment)
        {
            try
            {
                var result = await _addCommentRequestClient.Request(comment);

                if (result.Exception == null)
                {
                    return Created(nameof(AddComment), result);
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

        [HttpGet("comments")]
        public async Task<ActionResult<CommentEvent[]>> GetComments(string type, int parentId)
        {
            try
            {
                var result = await _getCommentRequestClient.Request(new GetComments { PageType = type, ParentId = parentId });
                return result;
            }
            catch (RequestTimeoutException)
            {
                return StatusCode((int)HttpStatusCode.RequestTimeout);
            }
        }
    }
}
