using MessageContracts;
using System;

namespace CatalogService.MessageContracts
{
    public class AlbumAddedToTag : IEvent
    {
        public int AlbumId { get; set; }
        public int TagId { get; set; }
        public string Type => nameof(AlbumAddedToTag);
        public DateTime CreatedAt { get; }

        public AlbumAddedToTag()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
