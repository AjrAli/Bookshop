using AutoMapper;
using Bookshop.Application.Features.Books;
using Bookshop.Domain.Entities;

namespace Bookshop.Application.Profiles.Books
{
    public class BookMappingProfile : Profile
    {
        public BookMappingProfile()
        {
            //Comment profile
            CreateMap<Comment, CommentResponseDto>()
                .ForPath(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.FirstName))
                .ForPath(dest => dest.UserName, opt => opt.MapFrom(src => src.Customer.IdentityData.UserName))
                .ForMember(dest => dest.DateComment, opt => opt.MapFrom(src => src.DateComment.ToShortDateString()));
            // Book profile
            CreateMap<Book, BookResponseDto>()
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language.ToString()))
                .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => src.PublishDate.ToShortDateString()))
                .ForPath(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
                .ForPath(dest => dest.AuthorAbout, opt => opt.MapFrom(src => src.Author.About))
                .ForPath(dest => dest.CategoryTitle, opt => opt.MapFrom(src => src.Category.Title));
        }
    }
}
