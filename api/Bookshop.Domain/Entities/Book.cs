using Bookshop.Domain.Common;
using Bookshop.Domain.Extension;
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

        [EnumDataType(typeof(Languages))]
        public Languages Language {  get; set; }
        public DateTime PublishDate { get; set; }
        private Book() { }
        private Book(Action<object, string> lazyLoader)
        {
            LazyLoader = lazyLoader;
        }
        public Book(string title, 
                    string description, 
                    string publisher, 
                    string isbn, 
                    decimal price, 
                    int quantity, 
                    int pageCount, 
                    string dimensions, 
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
        }
        public enum Languages
        {
            English,
            French,
            Dutch,
            German
        }

        // Relationships
        public long? AuthorId { get; set; }
        private Author _author;
        public Author Author
        {
            get => LazyLoader.Load(this, ref _author);
            set => _author = value;
        }
        public long CategoryId { get; set; }
        private Category _category;
        public Category Category
        {
            get => LazyLoader.Load(this, ref _category);
            set => _category = value;
        }
    }
}
