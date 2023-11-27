using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.Common
{
    public class ValidationConstants
    {

        //coach
        public const int CoachNameMinLength = 3;
        public const int CoachNameMaxLength = 40;

        public const int NationalityMinLength = 2;
        public const int NationalityMaxLength = 40;

        //footballer
        public const int FootballerNameMinLength = 2;
        public const int FootballerNameMaxLength = 40;


        //team
        public const int TeamNameMinLength = 3;
        public const int TeamNameMaxLength = 40;

        public const int TeamNationalityMinLength = 2;
        public const int TeamNationalityMaxLength = 40;


    }
}
