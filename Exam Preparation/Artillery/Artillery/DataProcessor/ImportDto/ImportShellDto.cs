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
    [XmlType("Shell")]
    public class ImportShellDto
    {
        [Required]
        [XmlElement("ShellWeight")]
        [Range(ValidationConstants.ShellWeightMinValue, ValidationConstants.ShellWeightMaxValue)]
        public double ShellWeight { get; set; }

        [Required]  
        [XmlElement("Caliber")]
        [MinLength(ValidationConstants.CaliberMinValue)]
        [MaxLength(ValidationConstants.CaliberMaxValue)]
        public string Caliber { get; set; } 
    }
}
