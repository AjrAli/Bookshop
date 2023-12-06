using AutoMapper;
using Bookshop.Application.Features.ShoppingCarts;
using Bookshop.Domain.Entities;

namespace Bookshop.Application.Profiles.ShoppingCarts
{
    public class ShoppingCartMappingProfile : Profile
    {
        public ShoppingCartMappingProfile()
        {

            // LineItem profile
            CreateMap<LineItem, ShopItemDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(source => source.BookId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(source => source.Quantity));
            CreateMap<ShopItemDto, LineItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(source => source.BookId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(source => source.Quantity));

            // ShoppingCart profile
            CreateMap<ShoppingCart, ShoppingCartDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(source => source.CustomerId))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(source => source.LineItems));
            CreateMap<ShoppingCartDto, ShoppingCart>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(source => source.CustomerId))
                .ForMember(dest => dest.LineItems, opt => opt.MapFrom(source => source.Items));
        }
    }
}
