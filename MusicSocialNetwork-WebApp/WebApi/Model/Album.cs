using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApi.Model.InternalModels;

namespace WebApi.Model
{
    public class Album
    {
        public long Id { get; set; }
        [JsonPropertyName("strAlbum")]
        public string Name { get; set; }
        public int YearReleased { get; set; }
        [JsonPropertyName("strAlbumThumb")]
        public string CoverArtUrl { get; set; }
        [JsonPropertyName("strDescriptionEN")]
        public string Description { get; set; }

        [JsonPropertyName("strMusicBrainzID")]
        public string MbId { get; set; }
        [JsonPropertyName("idAlbum")]
        public string TMDBId { get; set; }

        public long ArtistId { get; set; }
        public long? StyleId { get; set; }
        public long? GenreId { get; set; }
        public long? FormatId { get; set; }

        [ForeignKey(nameof(StyleId))]
        public Style Style { get; set; }
        [ForeignKey(nameof(GenreId))]
        public Genre Genre { get; set; }
        [ForeignKey(nameof(FormatId))]
        public Format Format { get; set; }
        [InverseProperty(nameof(Track.Album))]
        public ICollection<Track> Tracks { get; set; }

        [NotMapped]
        [JsonPropertyName("strStyle")]
        public string StyleStr { get; set; }
        [NotMapped]
        [JsonPropertyName("strGenre")]
        public string GenreStr { get; set; }
        [NotMapped]
        [JsonPropertyName("strReleaseFormat")]
        public string FormatStr { get; set; }
        [NotMapped]
        [JsonPropertyName("intYearReleased")]
        public string YearStr { get; set; }
    }
}
