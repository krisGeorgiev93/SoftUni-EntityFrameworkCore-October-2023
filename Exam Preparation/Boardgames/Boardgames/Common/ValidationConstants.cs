using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.Common
{
    public class ValidationConstants
    {

        //Boardgame
        public const int BoardgameNameMinLength = 10;
        public const int BoardgameNameMaxLength = 20;

        public const int RatingMinValue = 1;
        public const int RatingMaxValue = 10;

        public const int YearPublishedMinValue = 2018;
        public const int YearPublishedMaxValue = 2023;

        //Creator
        public const int FirstNameMinLength = 2;
        public const int FirstNameMaxLength = 7;

        public const int LastNameMinLength = 2;
        public const int LastNameMaxLength = 7;

        //Seller

        public const int NameMinLength = 5;
        public const int NameMaxLength = 20;

        public const int AddressMinLength = 2;
        public const int AddressMaxLength = 30;

    }
}
