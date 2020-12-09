using Aapie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Aapie.Controllers
{
    [ApiController]
    [Route("aapie")]
    public class AapieController : ControllerBase
    {
#nullable enable
        private readonly ILogger<AapieController> _logger;
        private readonly Database _database;

        public AapieController(ILogger<AapieController> logger, Database database)
        {
            _logger = logger;
            _database = database;
        }

        [HttpGet("get")]
        public async Task<ActionResult> GetUser(int id)
        {
            string? username = await GetAuthorizeUsername();
            var user = await _database.GetUser(id);

            if (username != null && user.Username.Equals(username))
            {
                return Ok(user);
            }
            else
            {
                return Problem("Nie hackke    ");
            }
        }

        [HttpPost("post")]
        public async Task<User> AddUser([FromBody] User user)
        {
            await _database.AddUser(user);
            return user;
        }
        private async Task<string?> GetAuthorizeUsername()
        {
            if (Request.Headers.ContainsKey("Authorization"))
            {
                var authHeaderVal = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                if (authHeaderVal.Scheme.Equals("basic",
                            StringComparison.OrdinalIgnoreCase) &&
                        authHeaderVal.Parameter != null)
                {
                    var encoding = Encoding.GetEncoding("iso-8859-1");
                    string value = encoding.GetString(Convert.FromBase64String(authHeaderVal.Parameter));
                    string[] values = value.Split(':');

                    string authUsername = values[0];
                    string authPassword = values[1];

                    if (await _database.CheckCredentials(authUsername, authPassword))
                    {
                        return authUsername;
                    }
                }
            }
            return null;
        }
    }

}