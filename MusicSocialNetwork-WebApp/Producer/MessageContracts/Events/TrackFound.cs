using CatalogService.MessageContract;
using MessageContract;
using System;

namespace MusicService.MessageContracts
{
    public class TrackFound : IEvent
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public double Duration { get; set; }
        public string Type => nameof(TrackFound);
        public DateTime CreatedAt { get; }

        public TrackFound()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
