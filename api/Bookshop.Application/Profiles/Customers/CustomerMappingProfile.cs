using AutoMapper;
using Bookshop.Application.Features.Customers;
using Bookshop.Application.Features.Customers.Commands.EditPassword;
using Bookshop.Application.Features.Customers.Commands.EditProfile;
using Bookshop.Domain.Entities;

namespace Bookshop.Application.Profiles.Customers
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            // Address profile
            CreateMap<Address, AddressDto>().ReverseMap();
            //Edit Profile profile
            CreateMap<Customer, EditProfileDto>().ReverseMap();
            //Edit Password profile
            CreateMap<Customer, EditPasswordDto>().ReverseMap();
            // Create Customer profile
            CreateMap<Customer, CustomerRequestDto>().ReverseMap();
            // Response Customer profile
            CreateMap<Customer, CustomerResponseDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(x => x.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(x => x.LastName))
                .ForPath(dest => dest.UserName, opt => opt.MapFrom(x => x.IdentityData.UserName))
                .ForPath(dest => dest.Email, opt => opt.MapFrom(x => x.IdentityData.Email))
                .ForPath(dest => dest.ShippingAddress, opt => opt.MapFrom(x => $"{x.ShippingAddress.Street}, {x.ShippingAddress.City}, {x.ShippingAddress.PostalCode}, {x.ShippingAddress.State}, {x.ShippingAddress.Country}"))
                .ForPath(dest => dest.BillingAddress, opt => opt.MapFrom(x => $"{x.BillingAddress.Street}, {x.BillingAddress.City}, {x.BillingAddress.PostalCode}, {x.BillingAddress.State}, {x.BillingAddress.Country}"))
                .ForPath(dest => dest.ShoppingCart.Items, opt => opt.MapFrom(x => x.ShoppingCart.LineItems))
                .ForPath(dest => dest.ShoppingCart.Total, opt => opt.MapFrom(x => x.ShoppingCart.Total))
                .ForPath(dest => dest.ShoppingCart.Id, opt => opt.MapFrom(x => x.ShoppingCart.Id))
                .AfterMap((src, dest) => dest.ShoppingCart = src.ShoppingCart != null && src.ShoppingCart.Id != 0 ? dest.ShoppingCart : null);
        }
    }
}
