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
            CreateMap<Category, CategoryResponseDto>().ReverseMap();
        }
    }
}
