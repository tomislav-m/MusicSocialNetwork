using System;

namespace MessageContracts
{
    public interface IEvent
    {
        public string Type { get; }
        public DateTime CreatedAt { get; }
    }
}
