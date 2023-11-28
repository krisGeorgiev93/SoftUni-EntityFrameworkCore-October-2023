using Artillery.Data.Models.Enums;
using Artillery.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Artillery.Common;

namespace Artillery.DataProcessor.ImportDto
{
    public class ImportGunDto
    {
        
        [Required]
        [JsonProperty("ManufacturerId")]
        public int ManufacturerId { get; set; }
       

        [Required]
        [JsonProperty("GunWeight")]
        [Range(ValidationConstants.GunWeightMinValue, ValidationConstants.GunWeightMaxValue)]
        public int GunWeight { get; set; }

        [Required]
        [JsonProperty("BarrelLength")]
        [Range(ValidationConstants.BarrelLengthMinValue, ValidationConstants.BarrelLengthMaxValue)]
        public double BarrelLength { get; set; }

        [JsonProperty("NumberBuild")]
        public int? NumberBuild { get; set; }

        [Required]
        [JsonProperty("Range")]
        [Range(ValidationConstants.RangeMinValue, ValidationConstants.RangeMaxValue)]
        public int Range { get; set; }

        [Required]
        [JsonProperty("GunType")]
        public string GunType { get; set; } 
   
        [Required]
        [JsonProperty("ShellId")]
        public int ShellId { get; set; }

        [JsonProperty("Countries")]
        public ImportCountriesDto[] Countries { get; set; } 
    }

    public class ImportCountriesDto
    {
        [JsonProperty("Id")]
        [Required]
        public int Id { get; set; }
    }
}
