using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theatre.Common
{
    public class ValidationConstants
    {

        //play
        public const int TitleMinLength = 4;
        public const int TitleMaxLength = 50;

        public const int PlayDurationMinRange = 1;

        public const double PlayRatingMinRange = 0.0;
        public const double PlayRatingMaxRange = 10.0;

        public const int DescriptionMaxValue = 700;

        public const int ScreenwriterMinLength = 4;
        public const int ScreenwriterMaxLength = 30;

        //cast
        public const int CastNameMinLength = 4;
        public const int CastNameMaxLength = 30;

        //theatre
        public const int TheatreNameMinLength = 4;
        public const int TheatreNameMaxLength = 30;

        public const sbyte NumberOfHallsMinValue = 1;
        public const sbyte NumberOfHallsMaxValue = 10;

        public const int DirectorMinLength = 4;
        public const int DirectorMaxLength = 30;

        //ticket
        public const double TicketPriceMinValue = 1;
        public const double TicketPricaMaxValue = 100;

        public const sbyte RowNumberMinValue = 1;
        public const sbyte RowNumberMaxValue = 10;

    }
}
