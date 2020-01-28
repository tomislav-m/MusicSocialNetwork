using System;
using System.Collections.Generic;

namespace Common.MessageContracts.Event.Commands
{
    public class AddEvent
    {
        public string Type { get; set; }
        public string Venue { get; set; }
        public DateTime Date { get; set; }
        public ICollection<int> Headliners { get; set; }
        public ICollection<int> Supporters { get; set; }
    }
}
