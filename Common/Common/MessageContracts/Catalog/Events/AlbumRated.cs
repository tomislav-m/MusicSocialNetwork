using System;

namespace Common.MessageContracts.Catalog.Events
{
    public class AlbumRated : IEvent
    {
        public int UserId { get; set; }
        public int AlbumId { get; set; }
        public float Rating { get; set; }
        public string Type => nameof(AlbumRated);
        public DateTime CreatedAt { get; set; }

        public AlbumRated()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
