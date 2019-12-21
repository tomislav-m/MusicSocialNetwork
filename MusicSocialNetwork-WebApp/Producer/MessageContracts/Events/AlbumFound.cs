using CatalogService.MessageContract;
using System;

namespace MusicService.MessageContracts
{
    public class AlbumFound : IEvent
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int YearReleased { get; set; }
        public string CoverArtUrl { get; set; }
        public string Type => nameof(AlbumFound);
        public DateTime CreatedAt { get; }

        public AlbumFound()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
