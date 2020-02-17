namespace Common.MessageContracts.User.Commands
{
    public class AddComment
    {
        public int Author { get; set; }
        public string Text { get; set; }
        public string PageType { get; set; }
        public int ParentId { get; set; }
    }
}
