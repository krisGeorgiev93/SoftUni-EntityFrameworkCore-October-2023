﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03_SalesDatabase.Data.Models
{
    public class Store
    {

        public Store()
        {
            this.Sales = new HashSet<Sale>();
        }

        [Key]
        public int StoreId { get; set; }

        [Unicode]
        [MaxLength(ValidatinConstants.StoreNameLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Sale> Sales { get; set; }

    }
}
