using AutoMapper;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.ShoppingCarts.Extension
{
    public static class ShoppingCartExtension
    {
        public static async Task<ShoppingCartResponseDto?> ToMappedShoppingCartDto(this ShoppingCart shoppingCart, BookshopDbContext context, IMapper mapper, CancellationToken cancellationToken)
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
        }
    }
}
