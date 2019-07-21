using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.Models
{
    public class BillProductProducts
    {
        public BillProduct billProduct { get; set; }
        public List<Product> products { get; set; }   
    }
}
