using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_HospitalDatabase.Data
{
    public class ValidationConstants
    {

        //patient
        public const int PatientFirstNameLength = 50;
        
        public const int PatientLastNameLength = 50;

        public const int AddressLength = 250;

        public const int EmailLength = 80;

        //Visitation
        public const int CommentsLength = 250;

        //Diagnose
        public const int DiagnoseNameLength = 50;
        public const int DiagnoseCommentsLength = 250;

        //Medicament
        public const int MedicamentNameLength = 50;
    }
}
