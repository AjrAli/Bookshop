using Bookshop.Domain.Common;

namespace Bookshop.Domain.Entities
{
    public class Comment : AuditableEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime DateComment { get; }


        private Comment() { }

        public Comment(string title, string content, int rating, Customer customer, Book book)
        {
            Title = title;
            Content = content;
            Rating = rating;
            DateComment = DateTime.Now;
            Customer = customer;
            Book = book;
        }



        //Relationships
        public Customer Customer { get; set; }
        public long CustomerId { get; set; }
        public Book Book { get; set; }
        public long BookId { get; set; }
    }
}
