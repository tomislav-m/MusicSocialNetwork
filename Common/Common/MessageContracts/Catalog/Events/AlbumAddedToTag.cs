namespace Common.MessageContracts.Catalog.Events
{
    public class AlbumAddedToTag : IEvent
    {
        public int AlbumId { get; set; }
        public int TagId { get; set; }
        public string Type => nameof(AlbumAddedToTag);
    }
}
