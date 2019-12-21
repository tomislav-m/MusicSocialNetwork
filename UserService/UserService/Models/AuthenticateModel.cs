using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Models
{
    public class AuthenticateModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
