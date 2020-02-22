using System;
using System.Collections.Generic;

namespace Common.MessageContracts.Catalog.Events
{
    public class Collection : IEvent
    {
        public IEnumerable<int> AlbumIds { get; set; }

        public string Type => nameof(Collection);

        public DateTime CreatedAt { get; set; }
        public Exception Exception { get; set; }
    }
}
