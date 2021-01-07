using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aapie
{
    public interface IUserService
    {
        Task<User> AddUser(User user);
        Task RemoveUser(string id);
        Task<User> GetUser(string id);
        Task<User> AlterUser(User user);
    }
}
