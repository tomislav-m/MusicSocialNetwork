using System.Collections.Generic;

namespace Common.MessageContracts.Music.Events
{
    public class SimilarAlbums
    {
        public ICollection<int> AlbumsIds { get; set; }
    }
}
