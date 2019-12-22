using EventService.Models;
using System;
using System.Collections.Generic;

namespace EventService.Events
{
    public class EventAdded
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public Venue Venue { get; set; }
        public DateTime Date { get; set; }
        public ICollection<EventParticipant> EventParticipant { get; set; }
    }
}
