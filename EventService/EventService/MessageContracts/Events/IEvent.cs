using System;

namespace EventService.MessageContracts
{
    public interface IEvent
    {
        public string Type { get; }
        public DateTime CreatedAt { get; }
    }
}
