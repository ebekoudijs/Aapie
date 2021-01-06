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
        public async Task<Order> AddOrder(Order order, User user)
        {
            return await _database.addOrder(order, user);
        }
        public async Task UpdateDeliveryDate(string orderId) {
            await _database.UpdateDeliveryDate(orderId);
        }
    }
}
