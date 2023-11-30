using Bookshop.Application.Features.Dto;

namespace Bookshop.Application.Features.Category
{
    public record CategoryDto : IBaseDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; }
    }
}
