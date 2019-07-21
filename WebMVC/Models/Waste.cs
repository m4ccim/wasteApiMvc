using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.Models
{
    public class Waste
    {
        [Key]
        public int WasteId { get; set; }
        public string Name { get; set; }
        public double RecyclingPrice { get; set; }
        public double StartAmount { get; set; }

    }
}
 