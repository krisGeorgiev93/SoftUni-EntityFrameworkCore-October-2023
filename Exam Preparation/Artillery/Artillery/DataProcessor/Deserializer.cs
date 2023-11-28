namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            XmlHelper xmlHelper = new XmlHelper();

            StringBuilder sb = new StringBuilder();

            ICollection<Country> validCountries = new HashSet<Country>();

            var countryDtos = xmlHelper.Deserialize<ImportCountryDto[]>(xmlString, "Countries");

            foreach (ImportCountryDto countryDto in countryDtos)
            {
                if (!IsValid(countryDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Country country = new Country()
                {
                    CountryName = countryDto.CountryName,
                    ArmySize = countryDto.ArmySize,
                };

                validCountries.Add(country);
                sb.AppendLine(String.Format(SuccessfulImportCountry, countryDto.CountryName, countryDto.ArmySize));
            }

            context.Countries.AddRange(validCountries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
           XmlHelper XmlHelper = new XmlHelper();

            StringBuilder sb = new StringBuilder();

            ICollection<Manufacturer> validManufacturers = new HashSet<Manufacturer>();

            var manufacturerDtos = XmlHelper.Deserialize<ImportManufacturerDto[]>(xmlString, "Manufacturers");

            foreach (ImportManufacturerDto manufacturerDto in manufacturerDtos)
            {
                if (!IsValid(manufacturerDto) || validManufacturers.Any(vm => vm.ManufacturerName == manufacturerDto.ManufacturerName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Manufacturer manufacturer = new Manufacturer()
                {
                    ManufacturerName = manufacturerDto.ManufacturerName,
                    Founded = manufacturerDto.Founded
                };

                string[] manufacturerInfo = manufacturerDto.Founded.Split(", ").ToArray();
                string townAndCountry = manufacturerInfo[manufacturerInfo.Length-2] + ", " + manufacturerInfo[manufacturerInfo.Length-1];
                validManufacturers.Add(manufacturer);
                sb.AppendLine(String.Format(SuccessfulImportManufacturer, manufacturerDto.ManufacturerName, townAndCountry));
            }

            context.Manufacturers.AddRange(validManufacturers);
            context.SaveChanges();  
            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            XmlHelper xmlHelper = new XmlHelper();

            StringBuilder sb = new StringBuilder();

            ICollection<Shell> validShells = new HashSet<Shell>();

            var shellDtos = xmlHelper.Deserialize<ImportShellDto[]>(xmlString, "Shells");

            foreach (ImportShellDto shellDto in shellDtos)
            {
                if (!IsValid(shellDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Shell shell = new Shell()
                {
                    ShellWeight = shellDto.ShellWeight,
                    Caliber = shellDto.Caliber,
                };

                validShells.Add(shell);
                sb.AppendLine(String.Format(SuccessfulImportShell, shellDto.Caliber, shellDto.ShellWeight));
            }

            context.Shells.AddRange(validShells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
           StringBuilder sb = new StringBuilder();

            ImportGunDto[] gunDtos = JsonConvert.DeserializeObject<ImportGunDto[]>(jsonString);

            ICollection<Gun> validGuns = new HashSet<Gun>();

            foreach (var gunDto in gunDtos)
            {

                if (!IsValid(gunDto) || !Enum.TryParse(gunDto.GunType, out GunType gunType))
                {
                    sb.AppendLine(ErrorMessage); 
                    continue;
                }
                Gun gun = new Gun()
                {
                    ManufacturerId = gunDto.ManufacturerId,
                    GunWeight = gunDto.GunWeight,
                    BarrelLength = gunDto.BarrelLength,
                    NumberBuild = gunDto.NumberBuild,
                    Range = gunDto.Range,
                    GunType = gunType,
                    ShellId = gunDto.ShellId,
                };

                //Select only the unique ContryIds
                foreach (ImportCountriesDto countryDto in gunDto.Countries.Distinct())
                {
                    //The Countries array will always contain valid ids.
                    gun.CountriesGuns.Add(new CountryGun()
                    {
                        CountryId = countryDto.Id
                    });


                }
                validGuns.Add(gun);
                sb.AppendLine(String.Format(SuccessfulImportGun, gun.GunType, gun.GunWeight, gun.BarrelLength));
            }
            context.Guns.AddRange(validGuns);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}