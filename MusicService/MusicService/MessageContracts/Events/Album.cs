using System.Collections.Generic;

namespace MusicService.MessageContracts
{
    public class Album
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
        public ICollection<TrackFound> Tracks { get; set; }
    }
}
