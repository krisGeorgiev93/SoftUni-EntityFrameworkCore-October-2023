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
    [XmlType("Boardgame")]
    public class ExportBoardgameDto
    {
        [Required]
        [XmlElement("BoardgameName")]
        [MinLength(ValidationConstants.BoardgameNameMinLength)]
        [MaxLength(ValidationConstants.BoardgameNameMaxLength)]
        public string Name { get; set; } = null!;     

        [Required]
        [XmlElement("BoardgameYearPublished")]
        [Range(ValidationConstants.YearPublishedMinValue, ValidationConstants.YearPublishedMaxValue)]
        public int YearPublished { get; set; }

    }
}
