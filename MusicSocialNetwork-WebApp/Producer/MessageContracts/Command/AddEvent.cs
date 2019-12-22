using EventService.Models;
using System;
using System.Collections.Generic;

namespace EventService.Commands
{
    public class AddEvent
    {
        public string Type { get; set; }
        public Venue Venue { get; set; }
        public DateTime Date { get; set; }
        public ICollection<EventParticipant> EventParticipant { get; set; }
    }
}
