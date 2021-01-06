using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aapie
{
    public class User
    {
        public User(string UID, string username, string gender, int age, string email, string password)
        {
            UserId = UID;
            Username = username;
            Gender = gender;
            Age = age;
            Email = email;
            Password = password;
        }

        public User()
        {
            
        }
        public void Gendercheck() {

            if (Gender == "Female")
            {
                Gender = "Vrouw";
            }
            else if (Gender == "Male")
            {
                Gender = "Man";
            }
            else if (Gender == "Other")
            {
                Gender = "Anders";
            }
            else
            {
                Gender = null;
            }
        }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
