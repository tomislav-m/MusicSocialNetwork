using MassTransit;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using UserService.Helpers;
using UserService.Services;
using Common.MessageContracts.User.Commands;
using Common.MessageContracts.User.Events;
using UserService.Models;
using AutoMapper;
using System;

namespace UserService.Consumers
{
    public class UserConsumer : IConsumer<SignInUser>, IConsumer<CreateUser>
    {
        private readonly IUserService _service;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly EventStoreService _eventStoreService;

        public UserConsumer(IUserService service, IOptions<AppSettings> appSettings, IMapper mapper, EventStoreService eventStoreService)
        {
            _service = service;
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _eventStoreService = eventStoreService;
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

        public async Task Consume(ConsumeContext<CreateUser> context)
        {
            var message = context.Message;
            var user = _service.Create(new User { Username = message.Username, Role = message.Role }, message.Password);
            var userCreated = _mapper.Map<User, UserCreated>(user);

            try
            {
                await context.RespondAsync(userCreated);
                _eventStoreService.AddEventToStream(userCreated, "user-stream");
            }
            catch (Exception exc)
            {
                userCreated.Exception = exc;
                await context.RespondAsync(userCreated);
            }
        }
    }
}
