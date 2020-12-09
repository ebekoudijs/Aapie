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
        public string PhoneNumber { get; set; }
        public User(string UName, string PWord, string PNumber)
        {
            Username = UName;
            Password = PWord;
            PhoneNumber = PNumber;
        }
        public User() { }
    }
}
