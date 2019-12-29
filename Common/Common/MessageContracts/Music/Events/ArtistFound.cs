namespace Common.MessageContracts.Music.Events
{
    public class ArtistFound : IEvent
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }

        public string Type => nameof(ArtistFound);
    }
}
