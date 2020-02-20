using System;

namespace CatalogService.Models
{
    public class AlbumRating
    {
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public int UserId { get; set; }
        public float Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
