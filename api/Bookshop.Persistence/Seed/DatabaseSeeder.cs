using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;

namespace Bookshop.Persistence.Seed
{
    public static class DatabaseSeeder
    {
        public static void Seed(BookshopDbContext context)
        {
            SeedAuthors(context);
            SeedCategories(context);
            SeedBooks(context);
            SeedOrders(context);
            SeedBookOrders(context);
            SeedShoppingCarts(context);
            SeedCartItems(context);
        }

        private static void SeedAuthors(BookshopDbContext context)
        {
            if (context.Authors != null && !context.Authors.Any())
            {
                var authors = new List<Author>
                {
                    new Author("Jake Paul", "He is a good author!"),
                    new Author("Steven Jones", "Best author!")
                };

                context.Authors.AddRange(authors);
                context.SaveChanges();
            }
        }
        private static void SeedCategories(BookshopDbContext context)
        {
            if (context.Categories != null && !context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category("category1", "best category"),
                    new Category("category2", "special category")
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
        }
        private static void SeedBooks(BookshopDbContext context)
        {
            if (context.Books != null && !context.Books.Any() &&
                context.Categories != null && context.Categories.Any() &&
                context.Authors != null && context.Authors.Any())
            {
                var books = new List<Book>
                {
                    new Book("book1","description1","publisher1","isbn1",12.3m,100,354,"5.3 X 8.2 X 1.0 inches | 0.7 pounds",
                             Book.Languages.English,new DateTime(2008, 3, 1, 7, 0, 0), context.Authors.FirstOrDefault().Id,
                             context.Categories.FirstOrDefault().Id),
                    new Book("book2","description2","publisher2","isbn2",89.5m,100,244,"9.3 X 4.2 X 2.0 inches | 0.9 pounds",
                             Book.Languages.French,new DateTime(2012, 6, 4, 12, 0, 0), context.Authors.OrderBy(x=> x.Id).LastOrDefault().Id,
                             context.Categories.OrderBy(x=> x.Id).LastOrDefault().Id)
                };

                context.Books.AddRange(books);
                context.SaveChanges();
            }
        }
        private static void SeedOrders(BookshopDbContext context)
        {
            if (context.Orders != null && !context.Orders.Any() &&
                context.Customers != null && context.Customers.Any())
            {
                var orders = new List<Order>
                {
                    new Order(21m,10m,200m,Order.CreditCards.Visa,DateTime.Now, Order.Status.Pending, context.Customers.FirstOrDefault().Id),
                    new Order(6m,5m,150m,Order.CreditCards.Mastercard,DateTime.Now, Order.Status.Completed, context.Customers.FirstOrDefault().Id)
                };

                context.Orders.AddRange(orders);
                context.SaveChanges();
            }
        }
        private static void SeedBookOrders(BookshopDbContext context)
        {
            if (context.BookOrders != null && !context.BookOrders.Any() &&
                context.Orders != null && context.Orders.Any() &&
                context.Books != null && context.Books.Any())
            {
                var bookOrders = new List<BookOrder>
                {
                    new BookOrder(10, context.Orders.FirstOrDefault().Id, context.Books.FirstOrDefault().Id),
                    new BookOrder(15, context.Orders.OrderBy(x=> x.Id).LastOrDefault().Id, context.Books.OrderBy(x=> x.Id).LastOrDefault().Id)
                };

                context.BookOrders.AddRange(bookOrders);
                context.SaveChanges();
            }
        }
        private static void SeedShoppingCarts(BookshopDbContext context)
        {
            if (context.ShoppingCarts != null && !context.ShoppingCarts.Any() &&
                context.Customers != null && context.Customers.Any())
            {
                var shoppingCarts = new List<ShoppingCart>
                {
                    new ShoppingCart(175m, context.Customers.FirstOrDefault().Id),
                    new ShoppingCart(115m, context.Customers.OrderBy(x=> x.Id).LastOrDefault().Id)
                };

                context.ShoppingCarts.AddRange(shoppingCarts);
                context.SaveChanges();
            }
        }
        private static void SeedCartItems(BookshopDbContext context)
        {
            if (context.CartItems != null && !context.CartItems.Any() &&
                context.ShoppingCarts != null && context.ShoppingCarts.Any() &&
                context.Books != null && context.Books.Any())
            {
                var cartItems = new List<CartItem>
                {
                    new CartItem(10, context.Books.FirstOrDefault().Id, context.ShoppingCarts.FirstOrDefault().Id),
                    new CartItem(15, context.Books.OrderBy(x=> x.Id).LastOrDefault().Id, context.ShoppingCarts.OrderBy(x=> x.Id).LastOrDefault().Id)
                };

                context.CartItems.AddRange(cartItems);
                context.SaveChanges();
            }
        }

    }
}
