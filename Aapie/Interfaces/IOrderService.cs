using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aapie
{
    public interface IOrderService
    {
        Task<Order> AddOrder(Order order, User user);
        Task UpdateDeliveryDate(string orderId);
    }
}
