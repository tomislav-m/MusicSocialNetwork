namespace MusicService.MessageContracts
{
    public abstract class Search
    {
        public string SearchTerm { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
    }

    public class SearchArtist : Search { }

    public class SearchAlbum : Search { }
}
