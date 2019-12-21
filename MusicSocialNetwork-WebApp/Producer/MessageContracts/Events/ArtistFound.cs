using CatalogService.MessageContract;
using System;

namespace MusicService.MessageContracts
{
    public class ArtistFound : IEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public string Type => nameof(ArtistFound);
        public DateTime CreatedAt { get; }

        public ArtistFound()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
