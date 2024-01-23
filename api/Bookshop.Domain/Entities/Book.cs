using Bookshop.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Bookshop.Domain.Entities
{
    public class Book : AuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Isbn { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int PageCount { get; set; }
        public string Dimensions { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }

        [EnumDataType(typeof(Languages))]
        public Languages Language { get; set; }
        public DateTime PublishDate { get; set; }
        private Book() { }
        public Book(string title,
                    string description,
                    string publisher,
                    string isbn,
                    decimal price,
                    int quantity,
                    int pageCount,
                    string dimensions,
                    string imageUrl,
                    string imageName,
                    Languages language,
                    DateTime publishDate,
                    Author author,
                    Category category)
        {
            Title = title;
            Description = description;
            Publisher = publisher;
            Isbn = isbn;
            Price = price;
            Quantity = quantity;
            PageCount = pageCount;
            Dimensions = dimensions;
            Language = language;
            PublishDate = publishDate;
            Author = author;
            Category = category;
            ImageUrl = imageUrl;
            ImageName = imageName;
        }

        public enum Languages
        {
            English,
            French,
            Dutch,
            German,
            Spanish
        }

        // Relationships
        public long AuthorId { get; set; }
        public Author Author { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
