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
            // LineItem Response profile
            CreateMap<LineItem, ShopItemResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(source => source.BookId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(source => source.Quantity))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(source => source.Price))
                .ForPath(dest => dest.Title, opt => opt.MapFrom(source => source.Book.Title))
                .ForPath(dest => dest.AuthorName, opt => opt.MapFrom(source => source.Book.Author.Name))
                .ForPath(dest => dest.CategoryTitle, opt => opt.MapFrom(source => source.Book.Category.Title));
            CreateMap<ShopItemResponseDto, LineItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(source => source.BookId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(source => source.Quantity));
            // Order profile
            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(source => source.LineItems))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(source => source.Total))
                .ForPath(dest => dest.VatRate, opt => opt.MapFrom(source => source.Customer.ShippingAddress.LocationPricing.VatRate))
                .ForPath(dest => dest.ShippingFee, opt => opt.MapFrom(source => source.Customer.ShippingAddress.LocationPricing.ShippingFee));
            CreateMap<OrderResponseDto, ShoppingCart>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.LineItems, opt => opt.MapFrom(source => source.Items));
        }
    }
}
