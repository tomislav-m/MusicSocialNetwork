using System.Collections.Generic;

namespace Common.MessageContracts.Recommender.Commands
{
    public class GetRecommendations
    {
        public IEnumerable<AlbumRating> AlbumRatings { get; set; }
    }

    public class AlbumRating
    {
        public int AlbumId { get; set; }
        public float Rating { get; set; }
        public ICollection<int> StyleIds { get; set; }
        public ICollection<int> GenreIds { get; set; }
    }
}
