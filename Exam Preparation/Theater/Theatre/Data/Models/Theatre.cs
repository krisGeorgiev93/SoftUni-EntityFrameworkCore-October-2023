﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theatre.Data.Models
{
    public class Theatre
    {

        public Theatre()
        {
            this.Tickets = new HashSet<Ticket>();
        }

        [Key]
        public int  Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public sbyte NumberOfHalls { get; set; }

        [Required]
        public string Director { get; set; } = null!;

        public virtual ICollection<Ticket> Tickets { get; set; } = null!;
    }
}
