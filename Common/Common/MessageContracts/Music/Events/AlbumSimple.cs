namespace Common.MessageContracts.Music.Events
{
    public class AlbumSimple
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CoverArtUrl { get; set; }
        public ArtistSimple Artist { get; set; }
    }
}
