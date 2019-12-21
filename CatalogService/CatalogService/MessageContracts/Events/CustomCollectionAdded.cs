using System;

namespace CatalogService.MessageContracts
{
    public class CustomTagAdded : IEvent
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Type => nameof(CustomTagAdded);
        public DateTime CreatedAt { get; }

        public CustomTagAdded()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
