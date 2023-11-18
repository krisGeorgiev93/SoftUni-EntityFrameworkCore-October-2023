using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Utilities;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();

            // string inputXml = File.ReadAllText(@"../../../Datasets/categories-products.xml");

            string result = GetUsersWithProducts(context);
            Console.WriteLine(result);
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            //create map
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            cfg.AddProfile<ProductShopProfile>()));

            XmlHelper xmlHelper = new XmlHelper();

            ImportUsersDto[] usersDtos =
                xmlHelper.Deserialize<ImportUsersDto[]>(inputXml, "Users");

            ICollection<User> users = new HashSet<User>();

            foreach (ImportUsersDto userDto in usersDtos)
            {
                // Manual mapping without AutoMapper
                //User user = new User()
                //{
                //    firstName = userDto.FirstName,
                //    secondName = userDto.SecondName
                //    age = userDto.Age
                //};

                User user = mapper.Map<User>(userDto);
                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>()));
            XmlHelper xmlHelper = new XmlHelper();

            //Deserializing the Xml to Product DTOs
            ImportProductsDto[] productDtos = xmlHelper.Deserialize<ImportProductsDto[]>(inputXml, "Products");

            //Mapping the Product DTOs to Products
            ICollection<Product> products = mapper.Map<Product[]>(productDtos);

            //Adding and Saving
            context.Products.AddRange(products);
            context.SaveChanges();

            //Output
            return $"Successfully imported {products.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>()));
            XmlHelper xmlHelper = new XmlHelper();

            ImportCategoriesDto[] categoryDtos = xmlHelper.Deserialize<ImportCategoriesDto[]>(inputXml, "Categories");

            ICollection<Category> validCategories = new HashSet<Category>();

            foreach (ImportCategoriesDto categoryDto in categoryDtos)
            {
                if (string.IsNullOrEmpty(categoryDto.Name))
                {
                    continue;
                }

                Category category = mapper.Map<Category>(categoryDto);
                validCategories.Add(category);
            }

            context.Categories.AddRange(validCategories);
            context.SaveChanges();

            return $"Successfully imported {validCategories.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>()));

            XmlHelper xmlHelper = new XmlHelper();

            ImportCategoryProductDto[] categoryProductDtos = xmlHelper.Deserialize<ImportCategoryProductDto[]>(inputXml, "CategoryProducts");

            ICollection<CategoryProduct> validCategoryProduct = new HashSet<CategoryProduct>();

            HashSet<int> productIds = context.Products.Select(p => p.Id).ToHashSet<int>();
            HashSet<int> categoryIds = context.Categories.Select(c => c.Id).ToHashSet<int>();

            foreach (var dto in categoryProductDtos)
            {
                //validations
                if (productIds.Contains(dto.ProductId) && categoryIds.Contains(dto.CategoryId))
                {
                    var categoryProduct = mapper.Map<CategoryProduct>(dto);
                    validCategoryProduct.Add(categoryProduct);
                }
            }

            //Adding and Saving
            context.CategoryProducts.AddRange(validCategoryProduct);
            context.SaveChanges();

            return $"Successfully imported {validCategoryProduct.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {

            IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<ProductShopProfile>()));

            XmlHelper xmlHelper = new XmlHelper();

            ExportProductDto[] productsInRange = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .Select(p => new ExportProductDto()
                {
                    Price = p.Price,
                    Name = p.Name,
                    BuyerName = p.Buyer.FirstName + " " + p.Buyer.LastName
                })
                .ToArray();

            return xmlHelper.Serialize<ExportProductDto[]>(productsInRange, "Products");
           
        }


        public static string GetSoldProducts(ProductShopContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var usersSoldProducts = context.Users
                .Where(u => u.ProductsSold.Count > 0)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .Select(u => new ExportUserDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold.Select(p => new ProductDto
                    {
                        Name = p.Name,
                        Price = p.Price,
                    }).ToArray()
                })
                .ToArray();

            return xmlHelper.Serialize<ExportUserDto[]>(usersSoldProducts, "Users");
        }


        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {

            XmlHelper xmlHelper = new XmlHelper();

            //IMapper mapper = CreateMapper();
            //With mapper --->
            //var categories = context.Categories
            //    .ProjectTo<ExportCategoryDto>(mapper.ConfigurationProvider)
            //    .OrderByDescending(p => p.ProductsCount)
            //    .ThenBy(p => p.TotalRevenue)
            //    .ToArray();

            var categories = context.Categories
                .Select(c => new ExportCategoryDto
                {
                    Name = c.Name,
                    Count = c.CategoryProducts.Count,
                    AveragePrice = c.CategoryProducts.Average(c => c.Product.Price),
                    TotalRevenue = c.CategoryProducts.Sum(c => c.Product.Price)
                })
                .OrderByDescending(p => p.Count)
                .ThenBy(p => p.TotalRevenue)
                .ToArray();

            return xmlHelper.Serialize<ExportCategoryDto[]>(categories, "Categories");

        }


        public static string GetUsersWithProducts(ProductShopContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();
            //Selecting the Users without auto mapper
            var usersInfo = context
                    .Users
                    .Where(u => u.ProductsSold.Any())
                    .OrderByDescending(u => u.ProductsSold.Count)
                    .Select(u => new UserInfo()
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age,
                        SoldProducts = new SoldProductsCount()
                        {
                            Count = u.ProductsSold.Count,
                            Products = u.ProductsSold.Select(p => new SoldProduct()
                            {
                                Name = p.Name,
                                Price = p.Price
                            })
                            .OrderByDescending(p => p.Price)
                            .ToArray()
                        }
                    })
                    .Take(10)
                    .ToArray();

            ExportUserCountDto exportUserCountDto = new ExportUserCountDto()
            {
                Count = context.Users.Count(u => u.ProductsSold.Any()),
                Users = usersInfo
            };

            //Output
            return xmlHelper.Serialize<ExportUserCountDto>(exportUserCountDto, "Users");


        }
    }

}
