using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aapie
{
    public class Order
    {
        public double Totalprice()
        {
            double totalprice = 12;
            return totalprice;
        }
        public Order()
        {

        }
        public List<OrderLine> OrderLines { get; set;}
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public Table Table { get; set; }
        public User User { get; set; }

    }
}
