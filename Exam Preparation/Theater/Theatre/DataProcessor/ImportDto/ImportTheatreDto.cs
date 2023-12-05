using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theatre.Common;
using Theatre.Data.Models;

namespace Theatre.DataProcessor.ImportDto
{
    public class ImportTheatreDto
    {

        [Required]
        [JsonProperty("Name")]
        [MinLength(ValidationConstants.TheatreNameMinLength)]
        [MaxLength(ValidationConstants.TheatreNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [JsonProperty("NumberOfHalls")]
        [Range(ValidationConstants.NumberOfHallsMinValue, ValidationConstants.NumberOfHallsMaxValue)]
        public sbyte NumberOfHalls { get; set; }


        [Required]
        [JsonProperty("Director")]
        [MinLength(ValidationConstants.DirectorMinLength)]
        [MaxLength(ValidationConstants.DirectorMaxLength)]
        public string Director { get; set; } = null!;

        [JsonProperty("Tickets")]
        public ImportTicketDto[] Tickets { get; set; }

    }

    public class ImportTicketDto
    {
        [Required]
        [JsonProperty("Price")]
        [Range(ValidationConstants.TicketPriceMinValue, ValidationConstants.TicketPricaMaxValue)]
        public decimal Price { get; set; }

        [Required]
        [JsonProperty("RowNumber")]
        [Range(ValidationConstants.RowNumberMinValue, ValidationConstants.RowNumberMaxValue)]
        public sbyte RowNumber { get; set; }

        [JsonProperty("PlayId")]
        public int PlayId { get; set; }

    }
}
