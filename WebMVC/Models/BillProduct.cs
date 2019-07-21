using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.Models
{
    public class BillProduct
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public double Amount { get; set; }
        public int BillId { get; set; }
        public Bill Bill { get; set; }
    }
}
