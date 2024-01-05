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
                .ForPath(dest => dest.ShippingAddress, opt => opt.MapFrom(x => $"{x.ShippingAddress.Street}, {x.ShippingAddress.PostalCode}, {x.ShippingAddress.City}, {x.ShippingAddress.Country}"))
                .ForPath(dest => dest.BillingAddress, opt => opt.MapFrom(x => $"{x.BillingAddress.Street}, {x.BillingAddress.PostalCode}, {x.BillingAddress.City}, {x.BillingAddress.Country}"))
                .ForPath(dest => dest.ShoppingCart.Items, opt => opt.MapFrom(x => x.ShoppingCart.LineItems))
                .ForPath(dest => dest.ShoppingCart.Total, opt => opt.MapFrom(x => x.ShoppingCart.Total))
                .AfterMap((src, dest) => dest.ShoppingCart = src.ShoppingCart != null && src.ShoppingCart.Id != 0 ? dest.ShoppingCart : null);
        }
    }
}
