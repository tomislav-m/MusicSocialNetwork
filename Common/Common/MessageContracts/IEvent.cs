using System;

namespace Common.MessageContracts
{
    public interface IEvent
    {
        public string Type { get; }
        public DateTime CreatedAt { get; set; }
        public Exception Exception { get; set; }
    }
}
