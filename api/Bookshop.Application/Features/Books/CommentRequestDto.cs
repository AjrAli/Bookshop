using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Books
{
    public class CommentRequestDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int Rating { get; set; }
        [JsonIgnore]
        public string? UserId { get; set; }
    }
}
