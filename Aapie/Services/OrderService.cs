using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aapie
{
    public class OrderService : IOrderService
    {
        private readonly Database _database;
        public OrderService(Database database) {
            _database = database;
        }
        public async Task<Order> AddOrder(Order order, string userId)
        {
            return await _database.AddOrder(order, userId);
        }
        public async Task SetDeliveryDate(string orderId) {
            await _database.SetDeliveryDate(orderId);
        }
        public async Task<List<Order>> GetOrders(string userid)
        {
            List<Order> Orders = await _database.GetOrders(userid);
            foreach (var order in Orders)
            {
                order.OrderLines = await _database.GetOrderLines(order.OrderId);
            }

            return Orders;
        }
    }
}
