using System.Collections.Generic;

namespace Common.MessageContracts.Recommender.Events
{
    public class Recommendations
    {
        public ICollection<int> ArtistIds { get; set; }
    }
}
