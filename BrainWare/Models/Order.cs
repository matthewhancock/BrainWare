using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrainWare.Models {
    public class Order {
        public int ID { get; set; }

        public string CompanyName { get; set; }

        public string Description { get; set; }

        public decimal Total => Items.Sum(q => q.Quantity * q.Price);

        public List<LineItem> Items { get; set; }

        public class LineItem {
            public Product Product { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }
    }
}