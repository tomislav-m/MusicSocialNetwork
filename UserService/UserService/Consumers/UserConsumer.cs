using MassTransit;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using UserService.Helpers;
using UserService.Services;
using Common.MessageContracts.User.Commands;
using Common.MessageContracts.User.Events;

namespace UserService.Consumers
{
    public class UserConsumer : IConsumer<SignInUser>
    {
        private readonly IUserService _service;
        private readonly AppSettings _appSettings;

        public UserConsumer(IUserService service, IOptions<AppSettings> appSettings)
        {
            _service = service;
            _appSettings = appSettings.Value;
        }

        public async Task Consume(ConsumeContext<SignInUser> context)
        {
            var message = context.Message;

            var user = await _service.Authenticate(message.Username, message.Password);

            if (user == null)
            {
                await context.RespondAsync(new UserSignedIn());
            }
            else
            {
                var tokenString = TokenHelper.CreateToken(_appSettings.Secret, user.Id);

                await context.RespondAsync(new UserSignedIn
                {
                    Id = user.Id,
                    Username = user.Username,
                    Token = tokenString
                });
            }
        }
    }
}
