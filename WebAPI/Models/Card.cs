using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Card
    {
        [Key]
        public int CardId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string CardOwnerName { get; set; }
        public double Balance { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
      
        public bool IsAdmin { get; set; }
    }
}
