using System;
using System.Collections.Generic;

namespace CatalogService.MessageContract
{
    public class AlbumAddedToCollection : IEvent
    {
        public int AlbumId { get; set; }
        public int CollectionId { get; set; }
        public string Type => nameof(AlbumAddedToCollection);
        public DateTime CreatedAt { get; }

        public AlbumAddedToCollection()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
