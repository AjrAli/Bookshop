using AutoMapper;
using Bookshop.Application.Features.ShoppingCarts;
using Bookshop.Domain.Entities;

namespace Bookshop.Application.Profiles.ShoppingCarts
{
    public class ShoppingCartMappingProfile : Profile
    {
        public ShoppingCartMappingProfile()
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

            // ShoppingCart Response profile
            CreateMap<ShoppingCart, ShoppingCartResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(source => source.CustomerId))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(source => source.LineItems));
            CreateMap<ShoppingCartResponseDto, ShoppingCart>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(source => source.CustomerId))
                .ForMember(dest => dest.LineItems, opt => opt.MapFrom(source => source.Items));


            // LineItem Request profile
            CreateMap<LineItem, ShopItemRequestDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(source => source.BookId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(source => source.Quantity));
            CreateMap<ShopItemRequestDto, LineItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(source => source.BookId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(source => source.Quantity));

            // ShoppingCart Request profile
            CreateMap<ShoppingCart, ShoppingCartRequestDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(source => source.CustomerId))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(source => source.LineItems));
            CreateMap<ShoppingCartRequestDto, ShoppingCart>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(source => source.CustomerId))
                .ForMember(dest => dest.LineItems, opt => opt.MapFrom(source => source.Items));
        }
    }
}
