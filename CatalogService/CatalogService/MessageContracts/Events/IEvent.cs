using System;

namespace CatalogService.MessageContracts
{
    public interface IEvent
    {
        public string Type { get; }
        public DateTime CreatedAt { get; }
    }
}
