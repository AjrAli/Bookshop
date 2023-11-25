﻿using Bookshop.Domain.Common;
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

        public BookshopDbContext(DbContextOptions<BookshopDbContext> options)
            : base(options)
        {
        }

        public BookshopDbContext(DbContextOptions<BookshopDbContext> options,
            ILoggedInUserService loggedInUserService)
            : base(options)
        {
            _loggedInUserService = loggedInUserService;
        }
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<Author> Authors => Set<Author>();

        public DbSet<Book> Books => Set<Book>();
        public DbSet<BookOrder> BookOrders => Set<BookOrder>();

        public DbSet<CartItem> CartItems => Set<CartItem>();
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
            modelBuilder.ApplyConfiguration(new BookOrderConfiguration());
            modelBuilder.ApplyConfiguration(new CartItemConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new ShoppingCartConfiguration());
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
                        entry.Entity.CreatedBy = _loggedInUserService?.UserId ?? "";
                        entry.Entity.LastModifiedBy = _loggedInUserService?.UserId ?? "";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _loggedInUserService?.UserId ?? "";
                        break;
                }
            }
        }
    }

    public static class BookshopDbContextExtension
    {
        public static async Task<Order> GetOrderByIdIncludeBookOrdersAsync(this BookshopDbContext context,
            long orderId)
        {
            return await context.Orders.Include(x => x.BookOrders).FirstOrDefaultAsync(x => x.Id == orderId);
        }
    }
}