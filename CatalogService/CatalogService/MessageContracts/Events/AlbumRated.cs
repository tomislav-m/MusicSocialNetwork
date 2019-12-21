using System;

namespace CatalogService.MessageContracts
{
    public class AlbumRated : IEvent
    {
        public int UserId { get; set; }
        public int AlbumId { get; set; }
        public float Rating { get; set; }
        public DateTime CreatedAt { get; }
        public string Type => nameof(AlbumRated);

        public AlbumRated()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
