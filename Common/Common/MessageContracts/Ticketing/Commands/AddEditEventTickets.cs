using System;
using System.Collections.Generic;
using System.Text;

namespace Common.MessageContracts.Ticketing.Commands
{
    public class AddEditEventTickets
    {
        public int EventId { get; set; }
        public int TicketsOverall { get; set; }
        public float Price { get; set; }
        public string Currency { get; set; }
    }
}
