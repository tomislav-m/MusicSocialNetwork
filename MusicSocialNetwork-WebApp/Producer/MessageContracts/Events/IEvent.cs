using System;

namespace MessageContract
{
    public interface IEvent
    {
        public string Type { get; }
        public DateTime CreatedAt { get; }
    }
}
