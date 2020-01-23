using System;

namespace Common.MessageContracts.Music.Events
{
    public class ArtistCreated : IEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string WebsiteUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string PhotoUrl { get; set; }
        public string Bio { get; set; }
        public int YearFormed { get; set; }
        public int YearBorn { get; set; }
        public string Country { get; set; }
        public string MbId { get; set; }

        public string Type => nameof(ArtistCreated);
        public DateTime CreatedAt { get; set; }
        public Exception Exception { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ArtistCreated()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
