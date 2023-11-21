using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Common
{
    public class ValidationConstants
    {
        //client
        public const int NameMinLength = 10;
        public const int NameMaxLength = 25;

        public const int NumberVatMinLength = 10;
        public const int NumberVatMaxLength = 15;

        //address
        public const int StreetNameMinLength = 10;
        public const int StreetNameMaxLength = 20;

        public const int CityMinLength = 5;
        public const int CityMaxLength = 15;

        public const int CountryMinLength = 5;
        public const int CountryMaxLength = 15;

        //invoice
        public const int InvoiceNumberMaxValue = 1500000000;
        public const int InvoiceNumberMinValue = 1000000000;

        //product
        public const int ProductNameMinLength = 9;
        public const int ProductNameMaxLength = 30;

        public const decimal ProductPriceMaxValue = 1000m;
        public const decimal ProductPriceMinValue = 5m;

    }
}
