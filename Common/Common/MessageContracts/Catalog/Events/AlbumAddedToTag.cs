using System;

namespace Common.MessageContracts.Catalog.Events
{
    public class AlbumAddedToCollection : IEvent
    {
        public int AlbumId { get; set; }
        public string Type => nameof(AlbumAddedToCollection);

        public DateTime CreatedAt { get; set; }

        public AlbumAddedToCollection()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
