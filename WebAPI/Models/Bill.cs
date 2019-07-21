using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Bill
    {
        [Key]
        public int BillId { get; set; }
        public int CardId { get; set; }
        public Card Card { get; set; }
        public DateTime DateTime { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }


        public bool IsCompleted { get; set; }
    }
}
