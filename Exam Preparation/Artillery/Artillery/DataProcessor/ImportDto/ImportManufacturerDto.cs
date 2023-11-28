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
    [XmlType("Manufacturer")]
    public class ImportManufacturerDto
    {

        [Required]
        [XmlElement("ManufacturerName")]
        [MinLength(ValidationConstants.ManufNameMinLength)]
        [MaxLength(ValidationConstants.ManufNameMaxLength)]
        public string ManufacturerName { get; set; } 

        [Required]
        [XmlElement("Founded")]
        [MinLength(ValidationConstants.FoundedMinValue)]
        [MaxLength(ValidationConstants.FoundedMaxValue)]
        public string Founded { get; set; } 
    }
}
