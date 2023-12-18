using Cadastre.Data.Enumerations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadastre.DataProcessor.ImportDtos
{
    public class ImportCitizenDto
    {

        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        [JsonProperty("FirstName")]
        public string FirstName { get; set; } = null!;

        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        [JsonProperty("LastName")]
        public string LastName { get; set; } = null!;

        [JsonProperty("BirthDate")]
        public string BirthDate { get; set; }

        [Required]
        [JsonProperty("MaritalStatus")]
        public string MaritalStatus { get; set; }

        [JsonProperty("Properties")]
        public int[] PropertiesIds { get; set; }
    }

   
}
