using Bookshop.Domain.Common;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Configurations;
using Bookshop.Persistence.Contracts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Bookshop.Domain.Entities.Customer;

namespace Bookshop.Persistence.Context
{
    public class BookshopDbContext : IdentityDbContext<IdentityUserData>
    {
        private readonly ILoggedInUserService? _loggedInUserService;
        private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        public BookshopDbContext(DbContextOptions<BookshopDbContext> options,
            ILoggedInUserService loggedInUserService)
            : base(options)
        {
            _loggedInUserService = loggedInUserService;
        }
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<LocationPricing> LocationPricings => Set<LocationPricing>();
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<LineItem> LineItems => Set<LineItem>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // use loggerFactory
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new AddressConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new LineItemConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new ShoppingCartConfiguration());
            modelBuilder.ApplyConfiguration(new LocationPricingConfiguration());
        }

        public override int SaveChanges()
        {
            UpdateAuditFieldsForEntities();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            UpdateAuditFieldsForEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditFieldsForEntities()
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _loggedInUserService?.GetUserId() ?? "";
                        entry.Entity.LastModifiedBy = _loggedInUserService?.GetUserId() ?? "";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _loggedInUserService?.GetUserId() ?? "";
                        break;
                }
            }
        }
    }

    public static class BookshopDbContextExtension
    {
        public static async Task<Order> GetOrderByIdIncludeLineItemsAsync(this BookshopDbContext context,
            long orderId)
        {
            return await context.Orders.Include(x => x.LineItems).FirstOrDefaultAsync(x => x.Id == orderId);
        }
        public static async Task<LocationPricing?> FindLocationPricingByCountry(this BookshopDbContext context, string country)
        {
            return await context.LocationPricings.FirstOrDefaultAsync(x => x.Country.ToUpper() == country.ToUpper() ||
                                                                              x.Country == "OTHERS");
        }
    }
}