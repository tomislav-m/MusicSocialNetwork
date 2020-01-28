using System;
using System.Collections.Generic;

namespace Common.MessageContracts.Event.Events
{
    public class EventAdded : IEvent
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public string Venue { get; set; }
        public DateTime Date { get; set; }
        public ICollection<int> Headliners { get; set; }
        public ICollection<int> Supporters { get; set; }
        public string Type => nameof(EventAdded);
        public DateTime CreatedAt { get; set; }
        public Exception Exception { get; set; }

        public EventAdded()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
