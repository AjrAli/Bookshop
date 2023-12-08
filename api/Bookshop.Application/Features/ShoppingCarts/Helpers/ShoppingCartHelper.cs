using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;

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
    }
}
