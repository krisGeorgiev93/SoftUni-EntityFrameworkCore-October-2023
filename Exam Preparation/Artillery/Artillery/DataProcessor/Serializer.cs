
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ExportDto;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {

            var shells = context.Shells
                .Where(s => s.ShellWeight > shellWeight)
                .ToArray()
                .Select(s => new
                {
                    ShellWeight = s.ShellWeight,    
                    Caliber = s.Caliber,
                    Guns = s.Guns
                    .Where(s => s.GunType.ToString() == "AntiAircraftGun")                    
                    .Select(g => new
                    {
                        GunType = g.GunType.ToString(),
                        GunWeight = g.GunWeight,
                        BarrelLength = g.BarrelLength,
                        Range = g.Range > 3000 ? "Long-range" : "Regular range"
                    })
                    .OrderByDescending(g => g.GunWeight)
                    .ToArray()
                })
                .OrderBy(s => s.ShellWeight)
                .ToArray();

            return JsonConvert.SerializeObject(shells, Formatting.Indented);
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
           XmlHelper xmlHelper = new XmlHelper();

            var guns = context.Guns
                .Where(g=> g.Manufacturer.ManufacturerName == manufacturer)
                .ToArray()
                .Select(g=> new ExportGunDto()
                {
                    Manufacturer = g.Manufacturer.ManufacturerName,
                    GunType = g.GunType.ToString(),
                    BarrelLength= g.BarrelLength,
                    GunWeight = g.GunWeight,
                    Range = g.Range,     
                    Countries = g.CountriesGuns
                    .Where(cg=> cg.Country.ArmySize > 4500000)
                    .Select(cg=> new ExportCountryDto()
                    {
                        Country = cg.Country.CountryName,
                        ArmySize = cg.Country.ArmySize,
                    })
                    .OrderBy(c=> c.ArmySize)
                    .ToArray()
                })
                .OrderBy(g=> g.BarrelLength) 
                .ToArray();


            return xmlHelper.Serialize<ExportGunDto[]>(guns, "Guns");
        }
    }
}
