﻿namespace Common.MessageContracts.User.Commands
{
    public class CreateUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}