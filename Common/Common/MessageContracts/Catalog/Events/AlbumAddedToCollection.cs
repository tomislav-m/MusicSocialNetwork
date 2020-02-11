using System;

namespace Common.MessageContracts.Catalog.Events
{
    public class AlbumAddedToCollection : IEvent
    {
        public int AlbumId { get; set; }
        public int UserId { get; set; }
        public string Type => nameof(AlbumAddedToCollection);

        public DateTime CreatedAt { get; set; }
        public Exception Exception { get; set; }

        public AlbumAddedToCollection()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
