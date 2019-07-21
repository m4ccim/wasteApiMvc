using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.Models
{
    public class BillProductWaste
    {
        [Key]
        public int Id { get; set; }
        public int BillProductId { get; set; }
        public BillProduct BillProduct { get; set; }
        public int WasteId { get; set; }
        public Waste Waste { get; set; }
        public double DiscountedAmount { get; set; }
    }
}
