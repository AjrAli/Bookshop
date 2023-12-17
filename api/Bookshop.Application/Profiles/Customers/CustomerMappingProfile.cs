using AutoMapper;
using Bookshop.Application.Features.Customers;
using Bookshop.Application.Features.Customers.Commands.EditCustomer;
using Bookshop.Application.Features.Orders;
using Bookshop.Application.Features.ShoppingCarts;
using Bookshop.Domain.Entities;

namespace Bookshop.Application.Profiles.Customers
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            // Address profile
            CreateMap<Address, AddressDto>().ReverseMap();
            //Edit Customer profile
            CreateMap<Customer, EditCustomerDto>().ReverseMap();
            // Create Customer profile
            CreateMap<Customer, CustomerRequestDto>().ReverseMap();
            // Response Customer profile
            CreateMap<Customer, CustomerResponseDto>().ReverseMap();
        }
    }
}
