﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery.Data.Models
{
    public class Country
    {
        public Country()
        {
            this.CountriesGuns = new HashSet<CountryGun>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string CountryName { get; set; } = null!;

        [Required]
        public int ArmySize { get; set; }

        [Required]
        public virtual ICollection<CountryGun> CountriesGuns { get; set; } = null!;
    }
}
