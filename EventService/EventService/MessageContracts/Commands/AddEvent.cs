using EventService.Models;
using System;
using System.Collections.Generic;

namespace EventService.MessageContracts
{
    public class AddEvent
    {
        public string Type { get; set; }
        public int VenueId { get; set; }
        public DateTime Date { get; set; }
        public ICollection<int> Headliners { get; set; }
        public ICollection<int> Supporters { get; set; }
    }
}
