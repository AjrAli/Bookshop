using Bookshop.Application.Contracts.Seed;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Service
{
    public class SeedApplicationService : ISeedApplicationService
    {
        private readonly BookshopDbContext _dbContext;

        public SeedApplicationService(BookshopDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SeedInfrastructureDataAsync()
        {
            await SeedLocationPricingsAsync();
            await SeedCustomerDataAsync();
        }


        public async Task SeedLocationPricingsAsync()
        {
            if (!await _dbContext.LocationPricings.AnyAsync() || await _dbContext.LocationPricings.CountAsync() == 1)
            {
                var listLocationPricing = new List<LocationPricing>
                {
                    new LocationPricing("Spain", 14.99m, 18m),
                    new LocationPricing("United Kingdom", 19.99m, 11m),
                    new LocationPricing("France", 4.99m, 12m),
                    new LocationPricing("Germany", 5.99m, 15m),
                    new LocationPricing("Italy", 19.99m, 19m),
                    new LocationPricing("OTHERS", 19.99m, 21m)
                };
                await _dbContext.LocationPricings.AddRangeAsync(listLocationPricing);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task SeedCustomerDataAsync()
        {
            if (await _dbContext.Customers.AnyAsync())
            {
                var customer = await _dbContext.Customers.Include(x => x.Orders).FirstOrDefaultAsync();
                if (customer != null && !customer.Orders.Any())
                {
                    var author1 = new Author("Jake Paul", "He is a good author!");
                    var author2 = new Author("Steven Jones", "Best author!");
                    var category1 = new Category("category1", "best category");
                    var category2 = new Category("category2", "special category");
                    var book = new Book("book1", "description1", "publisher1", "isbn1", 12.3m, 100, 354, "5.3 X 8.2 X 1.0 inches | 0.7 pounds", Book.Languages.English, new DateTime(2008, 3, 1, 7, 0, 0), author1, category1);
                    var book2 = new Book("book2", "description2", "publisher2", "isbn2", 89.5m, 100, 244, "9.3 X 4.2 X 2.0 inches | 0.9 pounds", Book.Languages.French, new DateTime(2012, 6, 4, 12, 0, 0), author2, category2);
                    var lineItems = new List<LineItem>()
                        {
                            new LineItem(book, 10),
                            new LineItem(book2, 15)
                        };
                    var shoppingCart = new ShoppingCart(customer, lineItems);
                    customer.ShoppingCart = shoppingCart;
                    var order1 = new Order(Order.CreditCards.Visa, customer, lineItems);
                    customer.Orders.Add(order1);
                    _dbContext.Customers.Update(customer);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
