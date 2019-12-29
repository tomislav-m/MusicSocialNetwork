namespace Common.MessageContracts.Ticketing.Commands
{
    public class BuyTickets
    {
        public int EventId { get; set; }
        public int Count { get; set; }
        public int UserId { get; set; }
    }
}
