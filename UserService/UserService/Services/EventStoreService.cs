using AutoMapper;
using Common.MessageContracts.User;
using Common.MessageContracts.User.Events;
using Common.Services;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Services
{
    public class EventStoreService : EventStoreServiceBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _service;

        public EventStoreService(IMapper mapper, IUserService service)
        {
            _mapper = mapper;
            _service = service;
            Init().Wait();
        }

        public async override Task RecreateDbAsync()
        {
            var events = await ReadFromStream("user-stream");
            foreach(var @event in events)
            {
                var type = @event.Event.EventType;

                switch (type)
                {
                    case UserMessageContracts.UserCreated:
                        var userCreated = JsonConvert.DeserializeObject<UserCreated>(
                            Encoding.UTF8.GetString(@event.Event.Data));
                        _service.Create(_mapper.Map<UserCreated, User>(userCreated));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
