using AutoMapper;
using Bookshop.Application.Features.Customers;
using Bookshop.Application.Features.Customers.Commands.EditCustomer;
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
            CreateMap<Customer, CustomerResponseDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(x => x.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(x => x.LastName))
                .ForMember(dest => dest.ShippingAddressId, opt => opt.MapFrom(x => x.ShippingAddressId))
                .ForMember(dest => dest.BillingAddressId, opt => opt.MapFrom(x => x.BillingAddressId))
                .ForPath(dest => dest.ShoppingCart.Items, opt => opt.MapFrom(x => x.ShoppingCart.LineItems))
                .ForPath(dest => dest.ShoppingCart.Total, opt => opt.MapFrom(x => x.ShoppingCart.Total))
                .AfterMap((src, dest) => dest.ShoppingCart = src.ShoppingCart != null && src.ShoppingCart.Id != 0 ? dest.ShoppingCart : null);
        }
    }
}
