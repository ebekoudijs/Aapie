using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aapie
{
    public class Product
    {
        public double Price { get; set; }
        public string Name { get; set; }
        public double AlcPercent { get; set; }
        public int Stock { get; set; }
        public double AmountCl { get; set; }
        public string Description { get; set; }
        public Product(string name, double price, double alcPercent, int stock, double amountCl, string description) {
            Price = price;
            Name = name;
            AlcPercent = alcPercent;
            Stock = stock;
            AmountCl = amountCl;
            Description = description;
        }
    }
}
