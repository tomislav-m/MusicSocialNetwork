namespace MusicService.MessageContracts
{
    public class AlbumFound
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int YearReleased { get; set; }
        public string CoverArtUrl { get; set; }
    }
}
