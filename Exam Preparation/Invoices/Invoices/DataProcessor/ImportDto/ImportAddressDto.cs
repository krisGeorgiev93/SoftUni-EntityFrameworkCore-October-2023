using Invoices.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Address")]
    public class ImportAddressDto
    {
        [XmlElement("StreetName")]
        [Required]
        [MinLength(ValidationConstants.StreetNameMinLength)]
        [MaxLength(ValidationConstants.StreetNameMaxLength)]
        public string StreetName { get; set; } = null!;

        [XmlElement("StreetNumber")]
        [Required]
        public int StreetNumber { get; set; }

        [XmlElement("PostCode")]
        [Required]
        public string PostCode { get; set; } = null!;

        [XmlElement("City")]
        [Required]
        [MinLength(ValidationConstants.CityMinLength)]
        [MaxLength(ValidationConstants.CityMaxLength)]
        public string City { get; set; } = null!;

        [XmlElement("Country")]
        [Required]
        [MinLength(ValidationConstants.CountryMinLength)]
        [MaxLength(ValidationConstants.CountryMaxLength)]
        public string Country { get; set; } = null!;
    }
}
