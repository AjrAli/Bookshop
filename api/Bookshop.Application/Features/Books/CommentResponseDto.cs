namespace Bookshop.Application.Features.Books
{
    public class CommentResponseDto
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int Rating { get; set; }
        public string? DateComment { get; set; }
        public string? CustomerName { get; set; }
        public string? UserName { get; set; }
    }
}
