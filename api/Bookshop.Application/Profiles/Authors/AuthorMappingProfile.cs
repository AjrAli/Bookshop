using AutoMapper;
using Bookshop.Application.Features.Authors;
using Bookshop.Domain.Entities;

namespace Bookshop.Application.Profiles.Authors
{
    public class AuthorMappingProfile : Profile
    {
        public AuthorMappingProfile()
        {

            // Author profile
            CreateMap<Author, AuthorResponseDto>().ReverseMap();
        }
    }
}
