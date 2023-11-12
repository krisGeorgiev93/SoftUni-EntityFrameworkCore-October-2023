using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace CarDealer
{
    public class StartUp
    {

        public static void Main()
        {

            CarDealerContext context = new CarDealerContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //string inputJson = File.ReadAllText(@"../../../Datasets/sales.json");
            string result = GetSalesWithAppliedDiscount(context);

            Console.WriteLine(result);

        }

        //Problem 09
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            var suppliersDtos = JsonConvert.DeserializeObject<Supplier[]>(inputJson);
            Supplier[] suppliers = mapper.Map<Supplier[]>(suppliersDtos);

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count()}.";

        }

        //Second method to resolve the problem 10 without mapping, just with validation
        //public static string ImportParts(CarDealerContext context, string inputJson)
        //{
        //    var parts = JsonConvert.DeserializeObject<Part[]>(inputJson)
        //        .Where(p => p.SupplierId <= 31);

        //    context.Parts.AddRange(parts);
        //    context.SaveChanges();

        //    return $"Successfully imported {parts.Count()}.";
        //}
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            //If the supplierId doesn't exist in the Suppliers table, skip the record.
            // Suppliers Ids starts from 1 to 31 !!!
            IMapper mapper = CreateMapper();

            ImportPartDto[] partDtos =
                JsonConvert.DeserializeObject<ImportPartDto[]>(inputJson);

            ICollection<Part> validParts = new HashSet<Part>();
            foreach (var partDto in partDtos)
            {
                if (partDto.SupplierId > 31 || partDto.SupplierId <= 0)
                {
                    continue;
                }

                Part part = mapper.Map<Part>(partDto);
                validParts.Add(part);
            }

            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}.";

        }

        //Problem 11
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            ImportCarDto[] importCarDtos = JsonConvert.DeserializeObject<ImportCarDto[]>(inputJson);
            ICollection<Car> carsToAdd = new HashSet<Car>();

            foreach (var carDto in importCarDtos)
            {
                Car currentCar = mapper.Map<Car>(carDto);

                foreach (var id in carDto.PartsId)
                {
                    if (!context.Parts.Any(p => p.Id == id))
                    {
                        continue;
                    }
                    //add the parts with their Ids
                    currentCar.PartsCars.Add(new PartCar
                    {
                        PartId = id,
                    });
                }

                carsToAdd.Add(currentCar);
            }

            context.Cars.AddRange(carsToAdd);
            context.SaveChanges();

            return $"Successfully imported {carsToAdd.Count}.";
        }

        //Problem 12
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();
            ImportCustomerDto[] customerDtos = JsonConvert.DeserializeObject<ImportCustomerDto[]>(inputJson);

            Customer[] customers = mapper.Map<Customer[]>(customerDtos);

            context.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}.";

        }

        //Problem 13
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            ImportSaleDto[] importSaleDtos = JsonConvert.DeserializeObject<ImportSaleDto[]>(inputJson);

            Sale[] salesToAdd = mapper.Map<Sale[]>(importSaleDtos);

            context.Sales.AddRange(salesToAdd);
            context.SaveChanges();
            return $"Successfully imported {salesToAdd.Count()}.";
        }

        //Problem 14
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(b => b.BirthDate)
                .ThenBy(d => d.IsYoungDriver)
                .Select(c => new
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = c.IsYoungDriver,
                }).ToList();

            return JsonConvert.SerializeObject(customers, Formatting.Indented);
        }

        //Problem 15
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(d => d.TraveledDistance)
                .Select(c => new
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                })
                .ToList();

            return JsonConvert.SerializeObject(cars, Formatting.Indented);
        }

        //Problem 16
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count()
                })
                .ToList();

            return JsonConvert.SerializeObject(suppliers, Formatting.Indented);
        }

        //Problem 17
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TraveledDistance
                    },
                    parts = c.PartsCars.Select(ps => new
                    {
                        Name = ps.Part.Name,
                        Price = $"{ps.Part.Price:f2}"
                    })

                })
                .ToList();

            return JsonConvert.SerializeObject(cars, Formatting.Indented);


        }

        //Problem 18
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Count() > 0)
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count(),
                    spentMoney = c.Sales.Sum(s => s.Car.PartsCars.Sum(p => p.Part.Price))
                })
                .OrderByDescending(x => x.spentMoney)
                .ThenByDescending(x => x.boughtCars)
                .ToArray();

            return JsonConvert.SerializeObject(customers, Formatting.Indented);
        }

        //Problem 19
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance,
                    },
                    customerName = s.Customer.Name,
                    discount = s.Discount.ToString("f2"),
                    price = (s.Car.PartsCars.Sum(x => x.Part.Price)).ToString("f2"),
                    priceWithDiscount = ((s.Car.PartsCars.Sum(x => x.Part.Price)) * (1 - (s.Discount / 100))).ToString("f2")

                })
                .ToList();

            return JsonConvert.SerializeObject(sales, Formatting.Indented);
        }

        //CreateMapperMethod
        private static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
        }
    }
}