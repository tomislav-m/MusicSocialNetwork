namespace Common.MessageContracts.Catalog.Events
{
    public class CustomTagAdded : IEvent
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Type => nameof(CustomTagAdded);
    }
}
