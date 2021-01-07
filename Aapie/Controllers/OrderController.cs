using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Aapie.Controllers
{
    [Route("order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly Database _database;
        public OrderController(Database database, IUserService userService, IOrderService orderService)
        {
            _database = database;
            _userService = userService;
            _orderService = orderService;
        }
        [HttpGet("drinks")]
        public async Task<List<Product>> GetProducts()
        {
            return await _database.GetProducts();
        }

        [HttpGet("orders")]
        public async Task<List<Aapie.Order>> GetOrders()
        {
            var user = await GetAuthorizeUser();
            return await _orderService.GetOrders(user.UserId);
        }

        [HttpPost("addorder")]
        public async Task<IActionResult> AddOrder([FromBody] Aapie.Order order)
        {
            var user = await GetAuthorizeUser();
            order.User = user;
            if (order.User != null)
            {
                await _database.AddOrder(order, order.User.UserId);
                return Ok(order);
            }
            else
            {
                return BadRequest("Invalid credentials");
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
