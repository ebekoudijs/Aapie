using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aapie
{
    public interface IOrderService
    {
        Task<Order> AddOrder(Order order, string userId);
        Task<List<Order>> GetOrders(string userid);
        Task SetDeliveryDate(string orderId);
    }
}
