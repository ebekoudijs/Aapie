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
        public async Task<User> GetUser(int id)
        {
            return await _database.GetUser(id);
        }
        
        [HttpPost("post")]
        public async Task<User> AddUser([FromBody] User user)
        {
            await _database.AddUser(user);
            return user;
        }
    }
    [ApiController]
    [Route("Arduino")]
    public class ArduinoController : ControllerBase
    {
        private readonly ArduinoDatabase _arduinodatabase;
        private readonly ILogger<ArduinoController> _logger;
        public ArduinoController(ILogger<ArduinoController> logger, ArduinoDatabase arduinodatabase)
        {
            _logger = logger;
            _arduinodatabase = arduinodatabase;
        }

        [HttpGet("get")]
        public async Task<string> Get(int left, int right, int straight, int id)
        {
            await _arduinodatabase.UpdateData(left, right, straight, id);
            return "test";
        }

        [HttpPost("post")]
        public async Task<User> Post([FromBody] User user)
        {
            
            return user;
        }
    }
}