using AutoMapper;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.ShoppingCarts.Extension
{
    public static class ShoppingCartExtension
    {
        public static string? GetQuantityMismatchMessage(this ShoppingCart shoppingCart, IList<ShopItemRequestDto> requestedItems)
        {
            var quantityMismatchMessages = requestedItems
                .Select(item =>
                {
                    var lineItem = shoppingCart.LineItems.FirstOrDefault(x => x.BookId == item.BookId && x.Quantity != item.Quantity);
                    return lineItem != null
                        ? $"Book {lineItem.Book.Title} quantity ordered: {item.Quantity} but received as max: {lineItem.Quantity}"
                        : null;
                })
                .Where(message => message != null)
                .ToList();

            return quantityMismatchMessages.Any() ? string.Join(", ", quantityMismatchMessages) : null;
        }
        public static async Task<ShoppingCartResponseDto?> LastSavedToMappedShoppingCartDto(this ShoppingCart shoppingCart, BookshopDbContext context, IMapper mapper, CancellationToken cancellationToken)
        {
            return await context.ShoppingCarts
                .Include(x => x.LineItems)
                    .ThenInclude(x => x.Book)
                        .ThenInclude(x => x.Author)
                .Include(x => x.LineItems)
                    .ThenInclude(x => x.Book)
                        .ThenInclude(x => x.Category)
                .Where(x => x.CustomerId == shoppingCart.CustomerId)
                .Select(x => mapper.Map<ShoppingCartResponseDto>(x))
                .FirstOrDefaultAsync(cancellationToken);
        }
        public static void RemoveShoppingCartFromCustomer(this ShoppingCart shoppingCart, BookshopDbContext context)
        {
            var customer = context.Customers.FirstOrDefault(x => x.Id == shoppingCart.CustomerId);
            if (customer != null)
            {
                customer.ShoppingCartId = null;
                context.Customers.Update(customer);
            }
        }
        public static void RemoveLineItems(this ShoppingCart shoppingCart, BookshopDbContext context)
        {
            foreach (var item in shoppingCart.LineItems)
                context.LineItems.Remove(item);
            shoppingCart.LineItems = null;
        }
        public static bool RemoveShoppingCartIfEmpty(this ShoppingCart shoppingCart, BookshopDbContext context)
        {
            if (shoppingCart.LineItems.Count == 0)
            {
                shoppingCart.RemoveShoppingCartFromCustomer(context);
                context.ShoppingCarts.Remove(shoppingCart);
                return true;
            }
            return false;
        }
        public static void RemoveShoppingCartReferencesWithLineItems(this ShoppingCart shoppingCart, BookshopDbContext context)
        {
            foreach (var lineItem in shoppingCart.LineItems)
                lineItem.ShoppingCartId = null;
            shoppingCart.LineItems = null;
            shoppingCart.UpdateShoppingCartTotal(context);
        }
        public static void UpdateShoppingCartTotal(this ShoppingCart shoppingCart, BookshopDbContext context)
        {
            shoppingCart.CalculateSubtotal();
            context.ShoppingCarts.Update(shoppingCart);
        }
    }
}
