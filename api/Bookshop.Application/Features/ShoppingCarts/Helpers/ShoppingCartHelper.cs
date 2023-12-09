using Bookshop.Application.Exceptions;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.ShoppingCarts.Helpers
{
    public static class ShoppingCartHelper
    {
        public static void RemoveShoppingCartFromCustomer(this ShoppingCart shoppingCart, BookshopDbContext context)
        {
            var customer = context.Customers.FirstOrDefault(x => x.Id == shoppingCart.CustomerId);
            if (customer != null)
            {
                customer.ShoppingCartId = null;
                context.Customers.Update(customer);
            }
        }
        public static async Task ValidateCommonShoppingCartRequest(this ShoppingCartRequestDto? shoppingCartDto, BookshopDbContext context)
        {
            if (shoppingCartDto == null)
                throw new ValidationException($"{nameof(shoppingCartDto)} is required.");

            if (shoppingCartDto.Items == null || !shoppingCartDto.Items.Any())
                throw new ValidationException("No items are listed in the ShoppingCart.");

            if (shoppingCartDto.CustomerId == null || shoppingCartDto.CustomerId == 0)
                throw new ValidationException("Customer undefined for the ShoppingCart.");

            if (!await context.Customers.AnyAsync(x => x.Id == shoppingCartDto.CustomerId))
                throw new ValidationException($"Customer {shoppingCartDto.CustomerId} doesn't exist");
        }
        public static async Task ValidateUpdateShoppingCartRequest(this ShoppingCartRequestDto? shoppingCartDto, BookshopDbContext context)
        {
            await shoppingCartDto.ValidateCommonShoppingCartRequest(context);
            if (shoppingCartDto.Id == 0)
                throw new ValidationException($"ShoppingCart {shoppingCartDto.Id} doesn't exist");

            if (!(await context.ShoppingCarts.AnyAsync(x => x.CustomerId == shoppingCartDto.CustomerId) &&
                await context.Customers.AnyAsync(x => x.ShoppingCartId == shoppingCartDto.Id)))
                throw new ValidationException($"ShoppingCart {shoppingCartDto.Id} with customer {shoppingCartDto.CustomerId} not found in Database");

            foreach (var item in shoppingCartDto.Items)
            {
                if (!await context.Books.AnyAsync(x => x.Id == item.BookId))
                    throw new ValidationException($"BookId: {item.BookId} not found in the database.");

                if (item.Quantity < 0 || (item.Quantity == 0 && item.Id == 0))
                    throw new ValidationException($"Invalid quantity: {item.Quantity} for BookId: {item.BookId}.");
            }
        }
        public static async Task ValidateCreateShoppingCartRequest(this ShoppingCartRequestDto? shoppingCartDto, BookshopDbContext context)
        {
            await shoppingCartDto.ValidateCommonShoppingCartRequest(context);
            if (await context.ShoppingCarts.AnyAsync(x => x.CustomerId == shoppingCartDto.CustomerId) ||
                await context.Customers.AnyAsync(x => x.ShoppingCartId == shoppingCartDto.Id))
                throw new ValidationException($"Customer {shoppingCartDto.CustomerId} already has a ShoppingCart.");

            foreach (var item in shoppingCartDto.Items)
            {
                if (!await context.Books.AnyAsync(x => x.Id == item.BookId))
                    throw new ValidationException($"BookId: {item.BookId} not found in the database.");

                if (item.Quantity <= 0)
                    throw new ValidationException($"Invalid quantity: {item.Quantity} for BookId: {item.BookId}.");
            }
        }
    }
}
