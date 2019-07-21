using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class ModelsContext : DbContext
    {
        public ModelsContext(DbContextOptions<ModelsContext> options) : base(options)
        {
            BillProducts.Include(x => x.Product).ToList();
             ProductWastes.Include(x => x.Waste).ToList();
            ProductWastes.Include(x => x.Product).ToList();

        }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Waste> Wastes { get; set; }
        public DbSet<CardWaste> CardWastes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductWaste> ProductWastes { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillProduct> BillProducts { get; set; }
        public DbSet<BillProductWaste> BillProductWastes { get; set; }
    }
}
