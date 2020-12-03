using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aapie.Controllers
{
    [ApiController]
    [Route("Aapie")]
    public class AapieController : ControllerBase
    {
        private readonly ILogger<AapieController> _logger;
        private readonly Database _database;

        public AapieController(ILogger<AapieController> logger, Database database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet("get")]
        public User Test()
        {
            //Database object creeren
            User NewUser = new User("pietpaulsema69", "johndeere420", "06 12345678");
            return NewUser;
        }

        [HttpPost("post")]
        public async Task<User> AddUser([FromBody] User user)
        {
            await _database.AddUser(user);
            return user;
        }
    }
}
