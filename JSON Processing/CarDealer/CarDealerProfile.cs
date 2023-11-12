using AutoMapper;
using CarDealer.DTOs.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //suppliers
            this.CreateMap<ImportSupplierDto, Supplier>();

            //parts
            this.CreateMap<ImportPartDto,Part>();

            //cars
            this.CreateMap<ImportCarDto, Car>();
          
            //customers
            this.CreateMap<ImportCustomerDto, Customer>();

            //sales
            this.CreateMap<ImportSaleDto, Sale>();
        }
    }
}
