using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theatre.Data.Models;
using Theatre.Common;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Cast")]
    public class ImportCastDto
    {
        [XmlElement("FullName")]
        [Required]
        [MinLength(ValidationConstants.CastNameMinLength)]
        [MaxLength(ValidationConstants.CastNameMaxLength)]
        public string FullName { get; set; } = null!;

        [Required]
        [XmlElement("IsMainCharacter")]
        public bool IsMainCharacter { get; set; }

        [Required]
        [RegularExpression(@"\+44-\d{2}-\d{3}-\d{4}")]
        [XmlElement("PhoneNumber")]
        public string PhoneNumber { get; set; } = null!;
        
        [Required]
        [XmlElement("PlayId")]
        public int PlayId { get; set; }
    }
}
