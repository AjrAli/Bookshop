using Bookshop.Domain.Common;

namespace Bookshop.Domain.Entities
{
    public class Author : AuditableEntity
    {
        public string Name { get; set; }
        public string About { get; set; }
        private Author() { }
        public Author (string name, string about)
        {
            Name = name;
            About = about;
        }

        // Relationships
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
