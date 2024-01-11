using Bookshop.Domain.Entities;
using Bookshop.Domain.Exceptions;

namespace Bookshop.Domain.Service
{
    public class StockService : IStockService
    {
        // Property to indicate whether the stock update was successful.
        public bool UpdateSucceded { get; set; }

        // Rollback stock quantities for the provided line items.
        public void RollbackStockQuantities(ICollection<LineItem> lineItems)
        {
            foreach (var lineItem in lineItems)
            {
                // Increase the stock quantity for each book in the line item.
                if (lineItem.Book != null)
                {
                    lineItem.Book.Quantity += lineItem.Quantity;
                }
            }
        }

        // Check if the stock quantities for the provided books are sufficient.
        public bool CheckBookStockQuantities(ICollection<Book> books)
        {
            List<Book> booksInvalid = new List<Book>();

            foreach (var book in books)
            {
                // Identify books with insufficient quantity.
                if (book != null && book.Quantity == 0)
                {
                    booksInvalid.Add(book);
                }
            }

            // If there are books with insufficient quantity, throw an exception.
            if (booksInvalid.Count > 0)
            {
                throw new InsufficientBookQuantityException(booksInvalid, "ShoppingCart update aborted");
            }

            // Stock quantities are sufficient.
            return true;
        }

        // Update stock quantities for the provided line items.
        public void UpdateStockQuantities(ICollection<LineItem> lineItems)
        {
            // Initialize the update status as unsuccessful.
            UpdateSucceded = false;

            Dictionary<Book, int> booksWithInvalidQt = new Dictionary<Book, int>();

            foreach (var lineItem in lineItems)
            {
                // Check and update stock quantities for each book in the line item.
                if (lineItem.Book != null)
                {
                    if (lineItem.Book.Quantity < lineItem.Quantity)
                    {
                        booksWithInvalidQt.Add(lineItem.Book, lineItem.Quantity);
                    }
                    else
                    {
                        lineItem.Book.Quantity -= lineItem.Quantity;
                    }
                }
            }

            // If there are books with insufficient quantity, throw an exception.
            if (booksWithInvalidQt.Count > 0)
            {
                // Identify the line items effectively updated and perform a rollback.
                var lineItemsEffectivlyUpdated = lineItems
                    .Where(x => !booksWithInvalidQt.Any(y => y.Key.Id == x.BookId))
                    .ToList();

                if (lineItemsEffectivlyUpdated?.Count > 0)
                {
                    RollbackStockQuantities(lineItemsEffectivlyUpdated);
                }

                throw new InsufficientBookQuantityException(booksWithInvalidQt, "ShoppingCart will be updated with correct quantity available");
            }

            // Update successful.
            UpdateSucceded = true;
        }
    }
}
