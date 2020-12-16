using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aapie
{
    public class User
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
       

        
        
        
        public User(string UID, string username, string gender, int age, string email, string password)
        {
            UserId = UID;
            Username = username;
            Gender = gender;
            Age = age;
            Email = email;
            Password = password;
            Gender = "Man";
            Age = 25;
            Email = "Test";
        }
        public User() {
            Gender = "Man";
            Age = 25;
            Email = "Test";
        }
    }
}
