using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Books
{
    public class CommentRequestDto
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int Rating { get; set; }
        public long BookId { get; set; }
        [JsonIgnore]
        public string? UserId { get; set; }
    }
}
