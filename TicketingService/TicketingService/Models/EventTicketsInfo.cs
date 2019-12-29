using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketingService.Models
{
    public class EventTicketsInfo
    {
        public int EventId { get; set; }
        public int TicketsOverall { get; set; }
        public int TicketsSold { get; set; }
        public float Price { get; set; }
        public string Currency { get; set; }
    }
}
