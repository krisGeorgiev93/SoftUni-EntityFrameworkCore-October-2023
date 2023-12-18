using Cadastre.Data.Enumerations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos
{
    [XmlType("District")]
    public class ImportDistrictDto
    {
        [Required]
        [MinLength(2)]
        [MaxLength(80)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[A-Z]{2}-\d{5}$")]
        [XmlElement("PostalCode")]
        public string PostalCode { get; set; } = null!;

        [Required]
        [XmlAttribute("Region")]       
        public string Region { get; set; }

        [XmlArray("Properties")]
        public ImportPropertyDto[] Properties { get; set; } = null!;
    }

    [XmlType("Property")]
    public class ImportPropertyDto
    {        
        [Required]
        [MinLength(16)]
        [MaxLength(20)]
        [XmlElement("PropertyIdentifier")]
        public string PropertyIdentifier { get; set; } = null!;

        [XmlElement("Area")]
        [Range(0, int.MaxValue)]
        public int Area { get; set; }

        [MinLength(5)]
        [MaxLength(500)]
        [XmlElement("Details")]
        public string? Details { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(200)]
        [XmlElement("Address")]
        public string Address { get; set; } = null!;

        [Required]
        [XmlElement("DateOfAcquisition")]
        public string DateOfAcquisition { get; set; }
    }
}
