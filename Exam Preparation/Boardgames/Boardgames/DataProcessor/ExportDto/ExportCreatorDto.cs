using Boardgames.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Creator")]
    public class ExportCreatorDto
    {
        [Required]
        [MinLength(ValidationConstants.FirstNameMinLength)]
        [MaxLength(ValidationConstants.FirstNameMaxLength)] 
        public string FirstName { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.LastNameMinLength)]
        [MaxLength(ValidationConstants.LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [XmlElement("CreatorName")]
        public string CreatorName { get; set; } = null!;

        [XmlAttribute("BoardgamesCount")]
        public int BoardgamesCount { get; set; }

        public ExportBoardgameDto[] Boardgames { get; set; } = null!;

    }
}
