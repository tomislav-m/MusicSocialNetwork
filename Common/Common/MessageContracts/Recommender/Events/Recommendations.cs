using System;
using System.Collections.Generic;

namespace Common.MessageContracts.Recommender.Events
{
    public class Recommendations : IEvent
    {
        public IEnumerable<int> AlbumIds { get; set; }

        public string Type => nameof(Recommendations);

        public DateTime CreatedAt { get; set; }

        public Exception Exception { get; set; }

        public Recommendations()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
