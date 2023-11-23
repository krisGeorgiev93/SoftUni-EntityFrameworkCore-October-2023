using Boardgames.Common;
using Boardgames.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Boardgame")]
    public class ImportBoardgameDto
    {
        [Required]
        [MinLength(ValidationConstants.BoardgameNameMinLength)]
        [MaxLength(ValidationConstants.BoardgameNameMaxLength)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [Required]
        [Range(ValidationConstants.RatingMinValue, ValidationConstants.RatingMaxValue)]
        [XmlElement("Rating")]
        public double Rating { get; set; }

        [Required]
        [Range(ValidationConstants.YearPublishedMinValue, ValidationConstants.YearPublishedMaxValue)]
        [XmlElement("YearPublished")]
        public int YearPublished { get; set; }

        [Required]
        [XmlElement("CategoryType")]
        public int CategoryType { get; set; }

        [Required]
        [XmlElement("Mechanics")]
        public string Mechanics { get; set; } = null!;

    }
}
