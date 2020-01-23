using System;

namespace Common.MessageContracts.User.Events
{
    public class UserSignedIn : IEvent
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string Type => nameof(UserSignedIn);
        public DateTime CreatedAt { get; set; }
        public Exception Exception { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public UserSignedIn()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
