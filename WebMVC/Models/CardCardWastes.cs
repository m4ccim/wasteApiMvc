using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.Models
{
    public class CardCardWastes
    {
        public Card Card { get; set; }
        public List<CardWaste> CardWastes { get; set; }
    }
}
