using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data.Models
{
    public class Medicament
    {
        public Medicament()
        {
            Prescriptions = new HashSet<PatientMedicament>();
        }
        [Key]
        public int MedicamentId { get; set; }

        [MaxLength(ValidationConstants.MedicamentNameLength)]
        public string Name { get; set; }

        //one to many
        public ICollection<PatientMedicament> Prescriptions { get; set; }
    }
}
