using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Theatre.Common;
using Theatre.Data.Models.Enums;

namespace Theatre.DataProcessor.ImportDto
{
    [XmlType("Play")]
    public class ImportPlayDto
    {
        [Required]
        [MinLength(4)]
        [MaxLength(50)]
        public string Title { get; set; }

        [Timestamp]
        [Required]
        public string Duration { get; set; }

        [Required]
        [Range(ValidationConstants.PlayRatingMinRange, ValidationConstants.PlayRatingMaxRange)]
        [XmlElement("Raiting")]
        public double Rating { get; set; }

        [Required]
        [EnumDataType(typeof(Genre))]
        public string Genre { get; set; }

        [Required]
        [MaxLength(700)]
        public string Description { get; set; }

        [Required]
        [MaxLength(30)]
        [MinLength(4)]
        public string Screenwriter { get; set; }
    }
}