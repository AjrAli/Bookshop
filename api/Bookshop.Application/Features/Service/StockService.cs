using Bookshop.Application.Exceptions;
using Bookshop.Domain.Entities;

namespace Bookshop.Application.Features.Service
{
    public class StockService : IStockService
    {
        public bool UpdateSucceded { get; set; }

        public void RollbackStockQuantities(ICollection<LineItem> lineItems)
        {
            foreach (var lineItem in lineItems)
            {
                if (lineItem.Book != null)
                {
                    lineItem.Book.Quantity += lineItem.Quantity;
                }
            }
        }
        public bool CheckBookStockQuantities(ICollection<Book> books)
        {

            List<Book> booksInvalid = new List<Book>();
            foreach (var book in books)
            {
                if (book != null)
                {
                    if (book.Quantity == 0)
                    {
                        booksInvalid.Add(book);
                    }
                }
            }
            if (booksInvalid.Count > 0)
            {
                throw new InsufficientBookQuantityException(booksInvalid, "ShoppingCart updated aborted");
            }
            return true;
        }
        public void UpdateStockQuantities(ICollection<LineItem> lineItems)
        {
            UpdateSucceded = false;

            Dictionary<Book, int> booksWithInvalidQt = new Dictionary<Book, int>();
            foreach (var lineItem in lineItems)
            {
                if (lineItem.Book != null)
                {
                    if (lineItem.Book.Quantity < lineItem.Quantity)
                    {
                        booksWithInvalidQt.Add(lineItem.Book, lineItem.Quantity);
                    }
                    else
                        lineItem.Book.Quantity -= lineItem.Quantity;
                }
            }
            if (booksWithInvalidQt.Count > 0)
            {
                var lineItemsEffectivlyUpdated = lineItems.Where(x => !booksWithInvalidQt.Any(y => y.Key.Id == x.BookId)).ToList();
                if (lineItemsEffectivlyUpdated?.Count > 0)
                    RollbackStockQuantities(lineItemsEffectivlyUpdated);
                throw new InsufficientBookQuantityException(booksWithInvalidQt, "ShoppingCart will be updated with correct quantity available");
            }
            UpdateSucceded = true;
        }
    }
}
