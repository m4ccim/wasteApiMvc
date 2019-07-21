using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Product
    {   [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double BasePrice { get; set; }
        public bool IsSoldByWeight { get; set; }
    }
}
