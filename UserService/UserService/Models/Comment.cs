using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int Author { get; set; }
        public string Text { get; set; }
        public string PageType { get; set; }
        public int ParentId { get; set; }

        [NotMapped]
        public string AuthorName { get; set; }
    }
}
