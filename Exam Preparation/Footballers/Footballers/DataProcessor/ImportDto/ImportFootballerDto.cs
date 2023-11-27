using Footballers.Common;
using Footballers.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Footballer")]
    public class ImportFootballerDto
    {

        [MinLength(ValidationConstants.FootballerNameMinLength)]
        [MaxLength(ValidationConstants.FootballerNameMaxLength)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [XmlElement("ContractStartDate")]
        [Required]
        public string? ContractStartDate { get; set; }

        [XmlElement("ContractEndDate")]
        [Required]
        public string? ContractEndDate { get; set; }

        [XmlElement("PositionType")]
        public int PositionType { get; set; }

        [XmlElement("BestSkillType")]
        public int BestSkillType { get; set; }
    }
}
