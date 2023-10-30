using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        public SalesContext()
        {
            
        }

        public SalesContext(DbContextOptions options) : base(options)
        {
            
        }

        DbSet<Customer> Customers { get; set; }
        DbSet<Sale> Sales { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Store> Stores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-6VUQ1VB\\SQLEXPRESS;Database=SalesDatabase;" +
                   "Trusted_Connection=true;TrustServerCertificate=true");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
