using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventService.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Venue { get; set; }
        public DateTime Date { get; set; }
        [NotMapped]
        public ICollection<int> Headliners { get; set; }
        [NotMapped]
        public ICollection<int> Supporters { get; set; }
    }
}
