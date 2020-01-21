using System;
using System.Collections.Generic;

namespace EventService.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Venue { get; set; }
        public DateTime Date { get; set; }
        public ICollection<int> Headliners { get; set; }
        public ICollection<int> Supporters { get; set; }
    }
}
