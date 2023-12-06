using AutoMapper;
using Bookshop.Application.Features.Customers;
using Bookshop.Domain.Entities;

namespace Bookshop.Application.Profiles.Customers
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            // Address profile
            // Customer profile
            CreateMap<Address, AddressDto>();
            // Customer profile
            CreateMap<Customer, CustomerDto>(MemberList.None);
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.IdentityUserDataId, opt => opt.MapFrom(source => source.IdentityUserDataId))
                .ForMember(dest => dest.BillingAddressId, opt => opt.MapFrom(source => source.BillingAddressId))
                .ForMember(dest => dest.ShippingAddressId, opt => opt.MapFrom(source => source.ShippingAddressId))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(source => source.IdentityData.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(source => source.IdentityData.Email))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(source => source.IdentityData.EmailConfirmed))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(source => source.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(source => source.LastName));
            CreateMap<CustomerDto, Customer>(MemberList.None);
            CreateMap<CustomerDto, Customer>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.IdentityUserDataId, opt => opt.MapFrom(source => source.IdentityUserDataId))
                .ForMember(dest => dest.BillingAddressId, opt => opt.MapFrom(source => source.BillingAddressId))
                .ForMember(dest => dest.ShippingAddressId, opt => opt.MapFrom(source => source.ShippingAddressId))
                .ForPath(dest => dest.IdentityData.UserName, opt => opt.MapFrom(source => source.Username))
                .ForPath(dest => dest.IdentityData.Email, opt => opt.MapFrom(source => source.Email))
                .ForPath(dest => dest.IdentityData.EmailConfirmed, opt => opt.MapFrom(source => source.EmailConfirmed))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(source => source.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(source => source.LastName));
        }
    }
}
