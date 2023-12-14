using Bookshop.Domain.Entities;

namespace Bookshop.Application.Exceptions
{
    public class InsufficientBookQuantityException : Exception
    {
        public List<Book> BooksInvalid { get; set; } = new List<Book>();
        public Dictionary<Book, int> BooksKeyWithQtValue { get; set; } = new Dictionary<Book, int>();
        private List<string> Errors { get; set; } = new List<string>();

        public override string Message => $"{base.Message}, (Errors: {string.Join(", ", Errors)})";
        public InsufficientBookQuantityException(List<Book> books, string message) : base(message)
        {
            BooksInvalid = books;
            foreach (var book in books)
            {
                Errors.Add($"Book {book.Title} out of stock");
            }
        }
        public InsufficientBookQuantityException(Dictionary<Book, int> booksKeyWithQtValue, string message) : base(message)
        {
            BooksKeyWithQtValue = booksKeyWithQtValue;
            foreach (var keyValuePair in booksKeyWithQtValue)
            {
                Errors.Add($"Book : {keyValuePair.Key.Title}, insufficient quantity => Stock value : {keyValuePair.Key.Quantity}, value ordered {keyValuePair.Value}");
            }
        }
    }
}
