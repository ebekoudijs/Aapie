using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Aapie
{
    public class UserService : IUserService
    {
        private readonly Database _database;
        public UserService(Database database) {
            _database = database;
        }

        public async Task<User> AddUser(User user)
        {
            return await _database.AddUser(user);
        }

        public Task<User> GetUser(string id)
        {
            return _database.GetUser(id);
        }
        public async Task RemoveUser(string id)
        {
            await _database.RemoveUser(id);
        }
        public async Task<User> ChangeUser(User user)
        {
            return new User();
        }
    }
}
