using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data.Models
{
    public class Diagnose
    {
        [Key]
        public int DiagnoseId { get; set; }

        [MaxLength(ValidationConstants.DiagnoseNameLength)]
        public string Name { get; set; }

        [MaxLength(ValidationConstants.DiagnoseCommentsLength)]
        public string Comments { get; set; }

        //relation with Patient
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
