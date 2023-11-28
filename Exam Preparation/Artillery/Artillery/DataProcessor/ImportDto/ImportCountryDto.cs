using Artillery.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Country")]
    public class ImportCountryDto
    {
        [Required]
        [XmlElement("CountryName")]
        [MinLength(ValidationConstants.CountryNameMinLength)]
        [MaxLength(ValidationConstants.CountryNameMaxLength)]
        public string CountryName { get; set; } = null!;

        [Required]
        [XmlElement("ArmySize")]
        [Range(ValidationConstants.ArmySizeMinValue, ValidationConstants.ArmySizeMaxValue)]
        public int ArmySize { get; set; }
    }
}
