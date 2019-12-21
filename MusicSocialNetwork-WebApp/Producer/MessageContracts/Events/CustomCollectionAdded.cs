using System;

namespace CatalogService.MessageContract
{
    public class CustomCollectionAdded : IEvent
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Type => nameof(CustomCollectionAdded);
        public DateTime CreatedAt { get; }

        public CustomCollectionAdded()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
