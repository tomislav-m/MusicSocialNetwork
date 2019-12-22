using MessageContracts;
using System;

namespace UserService.MessageContracts
{
    public class UserSignedIn : IEvent
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string Type => nameof(UserSignedIn);
        public DateTime CreatedAt { get; }
    }
}
