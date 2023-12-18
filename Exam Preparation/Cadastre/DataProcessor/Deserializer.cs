namespace Cadastre.DataProcessor
{
    using Cadastre.Data;
    using Cadastre.Data.Enumerations;
    using Cadastre.Data.Models;
    using Cadastre.DataProcessor.ImportDtos;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid Data!";
        private const string SuccessfullyImportedDistrict =
            "Successfully imported district - {0} with {1} properties.";
        private const string SuccessfullyImportedCitizen =
            "Succefully imported citizen - {0} {1} with {2} properties.";

        public static string ImportDistricts(CadastreContext dbContext, string xmlDocument)
        {
            XmlHelper xmlHelper = new XmlHelper();

            StringBuilder sb = new StringBuilder();

            var districtDtos = xmlHelper.Deserialize<ImportDistrictDto[]>(xmlDocument, "Districts");

            var allDistricts = dbContext.Districts.ToArray();
            var allProperties = dbContext.Properties.ToArray();

            ICollection<District> validDistricts = new HashSet<District>();

            foreach (var districtDto in districtDtos)
            {
                if (!IsValid(districtDto) || !Enum.TryParse(districtDto.Region, out Region region))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (allDistricts.Any(d => d.Name == districtDto.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                District district = new District()
                {

                    Region = region,
                    Name = districtDto.Name,
                    PostalCode = districtDto.PostalCode,
                };

                foreach (var propertyDto in districtDto.Properties)
                {
                    if (!IsValid(propertyDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (allProperties.Any(d => d.PropertyIdentifier == propertyDto.PropertyIdentifier
                        || d.Address == propertyDto.Address)
                        || district.Properties.Any(p => p.PropertyIdentifier == propertyDto.PropertyIdentifier || p.Address == propertyDto.Address))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool IsDateOfAcquisitionValid = DateTime.TryParseExact(propertyDto.DateOfAcquisition, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime propertyDate);

                    if (!IsDateOfAcquisitionValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Property property = new Property()
                    {
                        PropertyIdentifier = propertyDto.PropertyIdentifier,
                        Area = propertyDto.Area,
                        Details = propertyDto.Details,
                        Address = propertyDto.Address,
                        DateOfAcquisition = propertyDate
                    };

                    district.Properties.Add(property);
                }

                validDistricts.Add(district);
                sb.AppendLine(string.Format(SuccessfullyImportedDistrict, district.Name, district.Properties.Count()));
            }

            dbContext.Districts.AddRange(validDistricts);
            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();

        }

        public static string ImportCitizens(CadastreContext dbContext, string jsonDocument)
        {
            StringBuilder sb = new StringBuilder();

            ICollection<Citizen> validCitizens = new HashSet<Citizen>();

            var citizenDtos = JsonConvert.DeserializeObject<ImportCitizenDto[]>(jsonDocument);

            foreach (var citizenDto in citizenDtos)
            {
                if (!IsValid(citizenDto) || !Enum.TryParse(citizenDto.MaritalStatus, out MaritalStatus maritalStatus))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool IsBirthDateValid = DateTime.TryParseExact(citizenDto.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime citizenBirthDate);

                if (!IsBirthDateValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Citizen citizen = new Citizen()
                {
                    FirstName = citizenDto.FirstName,
                    LastName = citizenDto.LastName,
                    BirthDate = citizenBirthDate,
                    MaritalStatus = maritalStatus,
                };

                foreach (var propertyId in citizenDto.PropertiesIds)
                {
                    citizen.PropertiesCitizens.Add(new PropertyCitizen()
                    {
                        PropertyId = propertyId,
                    });
                }

                validCitizens.Add(citizen);
                sb.AppendLine(string.Format(SuccessfullyImportedCitizen, citizen.FirstName, citizen.LastName, citizen.PropertiesCitizens.Count()));

            }

            dbContext.Citizens.AddRange(validCitizens);
            dbContext.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
