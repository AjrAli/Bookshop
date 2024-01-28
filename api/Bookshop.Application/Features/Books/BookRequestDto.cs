using System.Text.Json.Serialization;

namespace Bookshop.Application.Features.Books
{
    public class BookRequestDto
    {
        public long? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Isbn { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int PageCount { get; set; }
        public string Dimensions { get; set; }
        private string? _imageUrl;
        public string? ImageUrl
        {
            get
            {
                return $"/client/img/{Isbn}";
            }
        }
        public string Language { get; set; }
        public string PublishDate { get; set; }
        public long AuthorId { get; set; }
        public long CategoryId { get; set; }
        [JsonIgnore]
        public byte[]? Image { get; set; }
        [JsonIgnore]
        public string? UploadImageDirectory { get; set; }
        [JsonIgnore]
        public string? ImageExtension { get; set; }
    }
}
