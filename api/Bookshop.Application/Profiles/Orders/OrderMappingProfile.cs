using AutoMapper;
using Bookshop.Application.Features.Orders;
using Bookshop.Application.Features.ShoppingCarts;
using Bookshop.Domain.Entities;

namespace Bookshop.Application.Profiles.Orders
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            // Order profile
            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(source => source.LineItems))
                .ForPath(dest => dest.VatRate, opt => opt.MapFrom(source => source.Customer.ShippingAddress.LocationPricing.VatRate))
                .ForPath(dest => dest.ShippingFee, opt => opt.MapFrom(source => source.Customer.ShippingAddress.LocationPricing.ShippingFee));
            CreateMap<OrderResponseDto, ShoppingCart>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.LineItems, opt => opt.MapFrom(source => source.Items))
                .ForAllMembers(x => x.Ignore());
        }
    }
}
