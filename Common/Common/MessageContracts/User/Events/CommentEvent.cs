using System;

namespace Common.MessageContracts.User.Events
{
    public class CommentEvent : IEvent
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public string PageType { get; set; }

        public string Type => nameof(CommentEvent);

        public DateTime CreatedAt { get; set; }
        public Exception Exception { get; set; }

        public CommentEvent()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
