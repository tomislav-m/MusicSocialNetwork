using System;

namespace CatalogService.MessageContract
{
    public interface IEvent
    {
        public string Type { get; }
        public DateTime CreatedAt { get; }
    }
}
