using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aapie
{
    public class OrderLine
    {
        public OrderLine(string orderid, int productId, int quantity) {

            ProductId = productId;
            Quantity = quantity;
            OrderId = orderid;
        
        }
        public string OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }   
}
