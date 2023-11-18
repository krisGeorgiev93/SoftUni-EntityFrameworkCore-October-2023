using AutoMapper;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //01.Import Users
            CreateMap<ImportUsersDto, User>();

            //02.Import Products
            CreateMap<ImportProductsDto, Product>();

            //03. Import Categories
            CreateMap<ImportCategoriesDto, Category>();
            
            //04. Import CategoryProduct
            CreateMap<ImportCategoryProductDto, CategoryProduct>();

            //export products in range
            CreateMap<Product, ExportProductDto>();

            //export users 
            CreateMap<User, ExportUserDto>();


        }
    }
}
