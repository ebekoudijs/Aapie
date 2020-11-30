using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aapie
{
    public class Order
    {
        public double Totalprice() {
            double totalprice = 12;
            return totalprice;
        }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public Table Table { get; set; }
        public List<Product> Products { get; set; }
        public string Status { get; set; }

        public Order(DateTime date, string message, Table table, List<Product> products, string status) {
            Date = date;
            Message = message;
            Table = table;
            Products = products;
            Status = status;
        }
    }   
}
