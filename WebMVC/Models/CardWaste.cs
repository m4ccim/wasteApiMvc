using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.Models
{
    public class CardWaste
    {
        [Key]
        public int CardWasteId { get; set; }
        public int CardId { get; set; }
        public Card Card { get; set; }
        public int WasteId { get; set; }
        public Waste Waste { get; set; }
        public double Amount { get; set; }
    }
}
