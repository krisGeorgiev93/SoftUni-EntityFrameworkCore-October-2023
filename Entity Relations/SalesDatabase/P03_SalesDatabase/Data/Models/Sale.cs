﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03_SalesDatabase.Data.Models
{
    public class Sale
    {
        [Key]
        public int SaleId { get; set; }

        public DateTime Date { get; set; }

        public virtual Product Product { get; set; }
        public int ProductId { get; set; }  

        public virtual Customer Customer { get; set; }
        public int CustomerId { get; set; }

        public virtual Store Store { get; set; }
        public int StoreId { get; set; }
    }
}
