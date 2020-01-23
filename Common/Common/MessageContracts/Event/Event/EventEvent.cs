﻿using System;
using System.Collections.Generic;

namespace Common.MessageContracts.Event.Events
{
    public class EventEvent : IEvent
    {
        public int Id { get; set; }
        public string EventType { get; set; }
        public string VenueName { get; set; }
        public DateTime Date { get; set; }
        public ICollection<int> Headliners { get; set; }
        public ICollection<int> Supporters { get; set; }
        public string Type => nameof(EventEvent);
        public DateTime CreatedAt { get; set; }
        public Exception Exception { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public EventEvent()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
