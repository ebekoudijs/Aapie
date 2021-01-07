﻿using Aapie;
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
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        public AapieController(ILogger<AapieController> logger, Database database, IUserService userService, IOrderService orderService)
        {
            _logger = logger;
            _database = database;
            _userService = userService;
            _orderService = orderService;

        }

        [HttpGet("getdrinks")]
        public async Task<List<Product>> GetProducts()
        {
            return await _database.GetProducts();
        }

        [HttpGet("getorders")]
        public async Task<List<Order>> GetOrders()
        {
            var user = await GetAuthorizeUser();
            return await _orderService.GetOrders(user.UserId);
        }

        [HttpPost("addorder")]
        public async Task<IActionResult?> AddOrder([FromBody] Order order)
        {
            var user = await GetAuthorizeUser();
            order.User = user;
            if (order.User != null)
            {
                await _database.AddOrder(order, order.User.UserId);
                return Ok(order);
            }
            else {
                return BadRequest("Invalid credentials");
            }
        }

        [HttpPost("createuser")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            user.Gendercheck();
            await _database.AddUser(user);
            return Ok(user);
        }
        
        [HttpGet("getuser")]
        public async Task<IActionResult> Login() {
            var user = await GetAuthorizeUser();

            if (user == null)
            {
                return Problem("Failed to authenticate");
            }
            else
            {
                return Ok(user);
            }
        }

        private async Task<User?> GetAuthorizeUser()
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

                    string authEmail = values[0];
                    string authPassword = values[1];
                    User dbuser = await _database.Authenticate(authEmail, authPassword);

                    if (dbuser != null)
                    {
                        return dbuser;
                    }
                }
            }
            return null;
        }
    }
}