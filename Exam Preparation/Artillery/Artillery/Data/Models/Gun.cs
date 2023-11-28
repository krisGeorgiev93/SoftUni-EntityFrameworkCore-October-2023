using Artillery.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery.Data.Models
{
    public class Gun
    {
        public Gun()
        {
            this.CountriesGuns = new HashSet<CountryGun>();
        }

        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Manufacturer))]
        [Required]
        public int ManufacturerId { get; set; }
        [Required]
        public Manufacturer Manufacturer { get; set; } = null!;

        [Required]
        public int GunWeight { get; set; }

        [Required]
        public double BarrelLength { get; set; }

        public int? NumberBuild { get; set; }

        [Required]
        public int Range { get; set; }

        [Required]
        public GunType GunType { get; set; }

        [ForeignKey(nameof(Shell))]
        [Required]
        public int ShellId { get; set; }
        [Required]
        public Shell Shell { get; set; } = null!;

        [Required]
        public virtual ICollection<CountryGun> CountriesGuns { get; set; } = null!;
    }
}
