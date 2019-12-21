using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApi.Model
{
    public class Artist
    {
        public long Id { get; set; }
        [JsonPropertyName("strArtist")]
        public string Name { get; set; }
        [JsonPropertyName("strWebsite")]
        public string WebsiteUrl { get; set; }
        [JsonPropertyName("strFacebook")]
        public string FacebookUrl { get; set; }
        [JsonPropertyName("strArtistThumb")]
        public string PhotoUrl { get; set; }
        [JsonPropertyName("strBiographyEN")]
        public string Bio { get; set; }
        public int YearFormed { get; set; }
        public int YearBorn { get; set; }
        [JsonPropertyName("strCountry")]
        public string Country { get; set; }

        public string MbId { get; set; }

        public ICollection<Album> Albums { get; set; }

        [NotMapped]
        [JsonPropertyName("intFormedYear")]
        public string YearStr { get; set; }
        [NotMapped]
        [JsonPropertyName("intBornYear")]
        public string YearBornStr { get; set; }
    }
}
