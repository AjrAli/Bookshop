using AutoMapper;
using Bookshop.Application.Features.Categories;
using Bookshop.Domain.Entities;

namespace Bookshop.Application.Profiles.Categories
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {

            // Category profile
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>(MemberList.None);
            CreateMap<CategoryDto, Category>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(source => source.Title))
                .ForMember(dest => dest.IsVisible, opt => opt.MapFrom(source => source.IsVisible))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(source => source.Description));
        }
    }
}
