using System;

namespace Common.MessageContracts.Music.Events
{
    public class ArtistFound : IEvent
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }

        public string Type => nameof(ArtistFound);
        public DateTime CreatedAt { get; set; }
        public Exception Exception { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ArtistFound()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
