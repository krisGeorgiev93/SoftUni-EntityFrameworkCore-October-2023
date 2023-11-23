using Boardgames.Common;
using Boardgames.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Creator")]
    public class ImportCreatorDto
    {
        [Required]
        [MinLength(ValidationConstants.FirstNameMinLength)]
        [MaxLength(ValidationConstants.FirstNameMaxLength)]
        [XmlElement("FirstName")]
        public string FirstName { get; set; } = null!;


        [Required]
        [MinLength(ValidationConstants.LastNameMinLength)]
        [MaxLength(ValidationConstants.LastNameMaxLength)]
        [XmlElement("LastName")]
        public string LastName { get; set; } = null!;

        [XmlArray("Boardgames")]
        public ImportBoardgameDto[] Boardgames { get; set; } = null!;


    }
}
