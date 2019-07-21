using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.Models
{
    public class ProductProductWastes
    {
        public Product Product { get; set; }
        public List<ProductWaste> productWastes {get; set;}
    }
}
