using Bookshop.Domain.Common;

namespace Bookshop.Domain.Entities
{
    public class Category : AuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; } = true;
        private Category() { }
        public Category(string title, string description) {  Title = title; Description = description; }

        // Relationships
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
