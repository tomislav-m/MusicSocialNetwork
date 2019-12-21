using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApi.Model
{
    public class Track
    {
        public long Id { get; set; }
        [JsonPropertyName("strTrack")]
        public string Title { get; set; }
        public double Duration { get; set; }

        public long AlbumId { get; set; }

        [ForeignKey(nameof(AlbumId))]
        public Album Album { get; set; }

        [NotMapped]
        [JsonPropertyName("intDuration")]
        public string DurationStr { get; set; }
    }
}
