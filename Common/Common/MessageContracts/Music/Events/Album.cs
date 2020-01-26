using System;
using System.Collections.Generic;

namespace Common.MessageContracts.Music.Events
{
    public class Album : IEvent
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int YearReleased { get; set; }
        public string CoverArtUrl { get; set; }
        public string Description { get; set; }
        public string MbId { get; set; }
        public string TMDBId { get; set; }
        public string Style { get; set; }
        public string Genre { get; set; }
        public string Format { get; set; }
        public int ArtistId { get; set; }
        public ICollection<Track> Tracks { get; set; }

        public string Type => nameof(Album);
        public DateTime CreatedAt { get; set; }
        public Exception Exception { get; set; }

        public Album()
        {
            CreatedAt = DateTime.Now;
        }
    }

    public class Track
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public float Duration { get; set; }
        public int AlbumId { get; set; }
    }
}
