using System;

namespace Common.MessageContracts.Catalog.Events
{
    public class AlbumAddedToTag : IEvent
    {
        public int AlbumId { get; set; }
        public int TagId { get; set; }
        public string Type => nameof(AlbumAddedToTag);

        public DateTime CreatedAt { get; set; }

        public AlbumAddedToTag()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
