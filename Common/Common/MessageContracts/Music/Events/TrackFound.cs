namespace Common.MessageContracts.Music.Events
{
    public class TrackFound : IEvent
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public double Duration { get; set; }

        public string Type => nameof(TrackFound);
    }
}
