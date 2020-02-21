using Common.MessageContracts;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public abstract class EventStoreServiceBase
    {
        private static IEventStoreConnection connection;

        public async Task Init()
        {
            if (connection == null)
            {
                while (true)
                {
                    try
                    {
                        connection = EventStoreConnection.Create(new Uri("tcp://admin:changeit@localhost:1113"));
                        await connection.ConnectAsync();
                        await RecreateDbAsync();
                        break;
                    }
                    catch
                    {
                        connection.Close();
                    }
                }
            }
        }

        public async Task<ResolvedEvent[]> ReadFromStream(string stream, int maxCount = 4095)
        {
            var eventsSlice = await connection.ReadStreamEventsBackwardAsync(stream, -1, maxCount, false);
            return eventsSlice.Events;
        }

        public async Task<ResolvedEvent[]> ReadFromStreamForward(string stream, int maxCount = 4095)
        {
            var eventsSlice = await connection.ReadStreamEventsForwardAsync(stream, 0, maxCount, false);
            return eventsSlice.Events;
        }

        public async void AddEventToStream(IEvent @event, string stream)
        {
            var myEvent = new EventData(Guid.NewGuid(), @event.Type, true,
                            Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event)),
                            Encoding.UTF8.GetBytes(@event.Type));

            await connection.AppendToStreamAsync(stream, ExpectedVersion.Any, myEvent);
        }

        public abstract Task RecreateDbAsync();
    }
}
