using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aapie
{
    public class Order
    {
        public Order()
        {
            OrderDate = DateTime.Now;
        }
        public Order(string orderId, DateTime orderDate, string message, int table) 
        {
            OrderId = orderId;
            OrderDate = orderDate;
            Message = message;
            Table = table;
        
        }
        public string OrderId { get; set; }
        public List<OrderLine> OrderLines { get; set;}
        public DateTime OrderDate { get; set; }
        public DateTime DateDelivered { get; set; }
        public string Message { get; set; }
        public int Table { get; set; }
        public User User { get; set; }
    }
}
