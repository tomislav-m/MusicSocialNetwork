using System;
using System.Collections.Generic;

namespace EventService.Models
{
    public class Event
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public Venue Venue { get; set; }
        public DateTime Date { get; set; }
        public ICollection<EventParticipant> EventParticipants { get; set; }
    }
}
