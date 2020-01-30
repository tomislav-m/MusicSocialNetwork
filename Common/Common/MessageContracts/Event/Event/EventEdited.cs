using System;
using System.Collections.Generic;

namespace Common.MessageContracts.Event.Event
{
    public class EventEdited : IEvent
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public string Venue { get; set; }
        public DateTime Date { get; set; }
        public ICollection<int> Headliners { get; set; }
        public ICollection<int> Supporters { get; set; }

        public string Type => nameof(EventEdited);
        public DateTime CreatedAt { get; set; }
        public Exception Exception { get; set; }

        public EventEdited()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
