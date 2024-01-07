using Bookshop.Application.Features.Dto;

namespace Bookshop.Application.Features.Categories
{
    public record CategoryResponseDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; }
    }
}
