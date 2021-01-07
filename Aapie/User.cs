using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aapie
{
    public class User
    {
        public User(string userId, string gender, int age, string email, string password, string name)
        {
            UserId = userId;
            Gender = gender;
            Age = age;
            Email = email;
            Password = password;
            Name = name;
        }

        public User(){}

        public void Gendercheck() {

            if (Gender.Equals("Female") )
            {
                Gender = "Vrouw";
            }
            else if (Gender.Equals("Male"))
            {
                Gender = "Man";
            }
            else if (Gender.Equals("Other"))
            {
                Gender = "Anders";
            }
            else
            {
                Gender = null;
            }
        }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
