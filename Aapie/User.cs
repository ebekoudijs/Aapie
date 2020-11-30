using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aapie
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public User(string UName, string PWord)
        {
            Username = UName;
            Password = PWord;
        }
    }
}
