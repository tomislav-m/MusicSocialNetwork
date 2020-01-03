using System;

namespace Common.MessageContracts.User.Events
{
    public class UserCreated : IEvent
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }

        public string Type => nameof(UserCreated);

        public DateTime CreatedAt { get; set; }

        public UserCreated()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
