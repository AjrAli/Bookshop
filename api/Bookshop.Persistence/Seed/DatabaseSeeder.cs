using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Persistence.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(BookshopDbContext context)
        {
            await SeedCustomerDataAsync(context);
        }

        private static async Task SeedCustomerDataAsync(BookshopDbContext context)
        {
            if (context.Customers.Any())
            {
                var customer = context.Customers.Include(x => x.Orders).FirstOrDefault();
                if (customer != null && !customer.Orders.Any())
                {
                    var author1 = new Author("Jake Paul", "He is a good author!");
                    var author2 = new Author("Steven Jones", "Best author!");
                    var category1 = new Category("category1", "best category");
                    var category2 = new Category("category2", "special category");
                    var book = new Book("book1", "description1", "publisher1", "isbn1", 12.3m, 100, 354, "5.3 X 8.2 X 1.0 inches | 0.7 pounds", Book.Languages.English, new DateTime(2008, 3, 1, 7, 0, 0));
                    var book2 = new Book("book2", "description2", "publisher2", "isbn2", 89.5m, 100, 244, "9.3 X 4.2 X 2.0 inches | 0.9 pounds", Book.Languages.French, new DateTime(2012, 6, 4, 12, 0, 0));
                    book.Author = author1;
                    book.Category = category1;
                    book2.Author = author2;
                    book2.Category = category2;
                    var lineItems = new List<LineItem>()
                        {
                            new LineItem(10) { Book = book },
                            new LineItem(15) { Book = book2 }
                        };
                    var shoppingCart = new ShoppingCart()
                    {
                        Customer = customer,
                        LineItems = lineItems
                    };
                    shoppingCart.CalculateTotal();
                    customer.ShoppingCart = shoppingCart;
                    var order1 = new Order(21m, 10m, Order.CreditCards.Visa, DateTime.Now, Order.Status.Pending)
                    {
                        Customer = customer,
                        LineItems = lineItems
                    };
                    order1.CalculateTotal();
                    customer.Orders.Add(order1);
                    context.Customers.Update(customer);
                    await context.SaveChangesAsync();
                }
            }
        }
    }

}
