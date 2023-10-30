using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data.Models
{
    public class Patient
    {
        public Patient()
        {
            Diagnoses = new HashSet<Diagnose>();
            Visitations = new HashSet<Visitation>();
            Prescriptions = new HashSet<PatientMedicament>();
        }
        public int PatientId { get; set; }

        [MaxLength(ValidationConstants.PatientFirstNameLength)]
        public string FirstName { get; set; } = null!;

        [MaxLength(ValidationConstants.PatientLastNameLength)]
        public string LastName { get; set; } = null!;

        [MaxLength(ValidationConstants.AddressLength)]
        public string Address { get; set; } = null!;

        [MaxLength(ValidationConstants.EmailLength)]
        public string Email { get; set; }

        public bool HasInsurance { get; set; }


        //one to many
        public ICollection<Diagnose> Diagnoses { get; set; }

        public ICollection<Visitation> Visitations { get; set; }

        public ICollection<PatientMedicament> Prescriptions { get; set; }
    }
}
