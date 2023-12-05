using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Theatre.Common;
using Theatre.Data.Models.Enums;

namespace Theatre.DataProcessor.ExportDto
{
    [XmlType("Play")]
    public class ExportPlayDto
    {

        [XmlAttribute("Title")]
        public string Title { get; set; } = null!;

        [XmlAttribute("Duration")]
        public string Duration { get; set; } = null!;

        [XmlAttribute("Rating")]
        public string Rating { get; set; }

        [XmlAttribute("Genre")]
        public string Genre { get; set; } = null!;

        [XmlAttribute("Description")]
        public string Description { get; set; } = null!;

        [XmlArray("Actors")]
        public ExportActorDto[] Actors { get; set; }

    }

    [XmlType("Actor")]
    public class ExportActorDto
    {
        [XmlAttribute("FullName")]
        public string FullName { get; set; } = null!;

        [XmlAttribute("MainCharacter")]
        public string IsMainCharacter { get; set; }

    }
}
