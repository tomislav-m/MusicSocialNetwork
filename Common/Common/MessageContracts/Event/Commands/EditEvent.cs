using System;
using System.Collections.Generic;
using System.Text;

namespace Common.MessageContracts.Event.Commands
{
    public class EditEvent
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Venue { get; set; }
        public DateTime Date { get; set; }
        public ICollection<int> Headliners { get; set; }
        public ICollection<int> Supporters { get; set; }
    }
}
