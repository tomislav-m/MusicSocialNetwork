namespace Common.MessageContracts.Music.Events
{
    public class AlbumFound : IEvent
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int YearReleased { get; set; }
        public string CoverArtUrl { get; set; }

        public string Type => nameof(AlbumFound);
    }
}
