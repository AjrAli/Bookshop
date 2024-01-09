using AutoMapper;
using Bookshop.Application.Features.Orders;
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
                .ForMember(dest => dest.StatusOrder, opt => opt.MapFrom(source => source.StatusOrder.ToString()))
                .ForMember(dest => dest.MethodOfPayment, opt => opt.MapFrom(source => source.MethodOfPayment.ToString()))
                .ForMember(dest => dest.DateOrder, opt => opt.MapFrom(source => source.DateOrder.ToShortDateString()))
                .ForPath(dest => dest.VatRate, opt => opt.MapFrom(source => source.Customer.ShippingAddress.LocationPricing.VatRate))
                .ForPath(dest => dest.ShippingFee, opt => opt.MapFrom(source => source.Customer.ShippingAddress.LocationPricing.ShippingFee));
        }
    }
}
