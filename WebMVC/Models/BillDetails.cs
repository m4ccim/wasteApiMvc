using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.Models
{
    public class BillDetails
    {
        public Bill Bill { get; set; }
        public List<ProductDetails> ProductDetails { get; set; }
    }

    public class ProductDetails
    {
        public Product Product { get; set; }
        public double Amount { get; set; }
        public List<WasteDiscount> WasteDiscounts { get; set; }
    }

    public class WasteDiscount
    {
        public Waste Waste { get; set; }
        public double DiscountedAmount { get; set; }
    }
}
