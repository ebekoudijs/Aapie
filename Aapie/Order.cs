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
            Date = DateTime.Now;
        }
        public List<OrderLine> OrderLines { get; set;}
        public DateTime Date { get; set; }
        public DateTime DateDelivered { get; set; }
        public string Message { get; set; }
        public int Table { get; set; }
        public User User { get; set; }
    }
}
