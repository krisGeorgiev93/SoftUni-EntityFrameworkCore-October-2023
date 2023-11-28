using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artillery.Common
{
    public class ValidationConstants
    {

        // country
        public const int CountryNameMinLength = 4;
        public const int CountryNameMaxLength = 60;

        public const int ArmySizeMinValue = 50000;
        public const int ArmySizeMaxValue = 10000000;

        //manufacturer
        public const int ManufNameMinLength = 4;
        public const int ManufNameMaxLength = 40;

        public const int FoundedMinValue = 10;
        public const int FoundedMaxValue = 100;

        //shell
        public const int ShellWeightMinValue = 2;
        public const int ShellWeightMaxValue = 1680;
        public const int CaliberMinValue = 4;
        public const int CaliberMaxValue = 30;

        //gun
        public const int GunWeightMinValue = 100;
        public const int GunWeightMaxValue = 1350000;
        public const double BarrelLengthMinValue = 2;
        public const double BarrelLengthMaxValue = 35;
        public const int RangeMinValue = 1;
        public const int RangeMaxValue = 100000;

    }
}
