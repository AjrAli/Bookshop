namespace Bookshop.Application.Features.Categories
{
    public class CategoryRequestDto
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
