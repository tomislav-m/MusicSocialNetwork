namespace Common.MessageContracts.User.Commands
{
    public class GetComments
    {
        public string PageType { get; set; }
        public int ParentId { get; set; }
    }
}
