using AutoMapper;
using Bookshop.Application.Features.Books;
using Bookshop.Domain.Entities;

namespace Bookshop.Application.Profiles.Books
{
    public class BookMappingProfile : Profile
    {
        public BookMappingProfile()
        {

            // Book profile
            CreateMap<Book, BookResponseDto>()
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language.ToString()))
                .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => src.PublishDate.ToShortDateString()))
                .ForPath(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
                .ForPath(dest => dest.AuthorAbout, opt => opt.MapFrom(src => src.Author.About))
                .ForPath(dest => dest.CategoryTitle, opt => opt.MapFrom(src => src.Category.Title))
                .ReverseMap();
        }
    }
}
