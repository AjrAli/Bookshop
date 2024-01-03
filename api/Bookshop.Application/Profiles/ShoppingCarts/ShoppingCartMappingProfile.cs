using AutoMapper;
using Bookshop.Application.Features.ShoppingCarts;
using Bookshop.Application.Features.ShoppingCarts.Queries.GetShoppingCartDetails;
using Bookshop.Domain.Entities;

namespace Bookshop.Application.Profiles.ShoppingCarts
{
    public class ShoppingCartMappingProfile : Profile
    {
        public ShoppingCartMappingProfile()
        {

            // LineItem Response profile
            CreateMap<LineItem, ShopItemResponseDto>()
                .ForPath(dest => dest.Title, opt => opt.MapFrom(source => source.Book.Title))
                .ForPath(dest => dest.AuthorName, opt => opt.MapFrom(source => source.Book.Author.Name))
                .ForPath(dest => dest.CategoryTitle, opt => opt.MapFrom(source => source.Book.Category.Title))
                .ForPath(dest => dest.BookPrice, opt => opt.MapFrom(source => source.Book.Price))
                .ForPath(dest => dest.ImageUrl, opt => opt.MapFrom(source => source.Book.ImageUrl));
            CreateMap<ShopItemResponseDto, LineItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(source => source.BookId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(source => source.Quantity));
            // ShoppingCart Response profile
            CreateMap<ShoppingCart, ShoppingCartResponseDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(source => source.LineItems))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(source => source.Total));
            CreateMap<ShoppingCartResponseDto, ShoppingCart>()
                .ForMember(dest => dest.LineItems, opt => opt.MapFrom(source => source.Items));

            // ShoppingCart Full details profile
            CreateMap<ShoppingCart, GetShoppingCartDetailsResponseDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(source => source.LineItems))
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(source => source.Total))
                .ForPath(dest => dest.VatRate, opt => opt.MapFrom(source => source.Customer.ShippingAddress.LocationPricing.VatRate))
                .ForPath(dest => dest.ShippingFee, opt => opt.MapFrom(source => source.Customer.ShippingAddress.LocationPricing.ShippingFee));
            CreateMap<GetShoppingCartDetailsResponseDto, ShoppingCart>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.LineItems, opt => opt.MapFrom(source => source.Items));

            // LineItem Request profile
            CreateMap<LineItem, ShopItemRequestDto>();
            CreateMap<ShopItemRequestDto, LineItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(source => source.BookId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(source => source.Quantity));

            // ShoppingCart Request profile
            CreateMap<ShoppingCart, ShoppingCartRequestDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(source => source.LineItems));
            CreateMap<ShoppingCartRequestDto, ShoppingCart>()
                .ForMember(dest => dest.LineItems, opt => opt.MapFrom(source => source.Items));
        }
    }
}
