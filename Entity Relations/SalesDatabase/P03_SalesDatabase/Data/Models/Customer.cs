using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03_SalesDatabase.Data.Models
{
    public class Customer
    {
        public Customer()
        {
            this.Sales = new HashSet<Sale>();
        }
        [Key]
        public int CustomerId { get; set; }

        [Unicode]
        [MaxLength(ValidatinConstants.CustomerNameLength)]
        public string Name { get; set; } = null!;

        [Unicode(false)]
        [MaxLength(ValidatinConstants.EmailCustomerLength)]
        public string Email { get; set; }

        public string CreditCardNumber { get; set; }

        public virtual ICollection<Sale> Sales { get; set; }
    }
}
