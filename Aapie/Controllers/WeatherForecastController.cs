using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aapie.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase


    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly Database _database;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, Database database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }





        [HttpGet("test")]

        public User Test()
        {
            //Database object creeren
            User NewUser = new User("pietpaulsema69", "johndeere420", "06 12345678");
            return NewUser;
        }
        [HttpPost("post")]
        public async Task<ActionResult> AddUser([FromBody] User user)
        {

           
            await _database.AddUser(user);


            return Ok("Goeie");
        }
    }
}
