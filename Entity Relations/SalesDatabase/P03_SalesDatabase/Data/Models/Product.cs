using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03_SalesDatabase.Data.Models
{
    public class Product
    {
        public Product()
        {
            this.Sales = new HashSet<Sale>();
        }

        [Key]
        public int ProductId { get; set; }

        [Unicode]
        [MaxLength(ValidatinConstants.ProductNameLength)]
        public string Name { get; set; } = null!;

        public double Quantity { get; set; }

        public decimal Price { get; set; } 

        public virtual ICollection<Sale> Sales { get; set; }
    }
}
