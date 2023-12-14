using Bookshop.Application.Exceptions;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.ShoppingCarts.Validation
{
    public static class ShoppingCartValidation
    {
        public static void ValidateCommonShoppingCartRequest(this ShoppingCartRequestDto? shoppingCartDto)
        {
            if (shoppingCartDto == null)
                throw new ValidationException($"{nameof(shoppingCartDto)} is required.");

            if (shoppingCartDto.Items == null || !shoppingCartDto.Items.Any())
                throw new ValidationException("No items are listed in the ShoppingCart.");
        }
        public static async Task ValidateUpdateShoppingCartRequest(this ShoppingCartRequestDto? shoppingCartDto, BookshopDbContext context)
        {
            shoppingCartDto.ValidateCommonShoppingCartRequest();
            if (!await context.ShoppingCarts.Include(x => x.Customer).AnyAsync(x => x.Customer.IdentityUserDataId == shoppingCartDto.UserId))
                throw new ValidationException($"ShoppingCart of current customer not found in Database");

            foreach (var item in shoppingCartDto.Items)
            {
                if (!await context.Books.AnyAsync(x => x.Id == item.BookId))
                    throw new ValidationException($"BookId: {item.BookId} not found in the database.");

                if (item.Quantity < 0 || item.Quantity == 0 && item.Id == 0)
                    throw new ValidationException($"Invalid quantity: {item.Quantity} for BookId: {item.BookId}.");
            }
        }
        public static async Task ValidateCreateShoppingCartRequest(this ShoppingCartRequestDto? shoppingCartDto, BookshopDbContext context)
        {
            shoppingCartDto.ValidateCommonShoppingCartRequest();
            if (await context.ShoppingCarts.Include(x => x.Customer).AnyAsync(x => x.Customer.IdentityUserDataId == shoppingCartDto.UserId))
                throw new ValidationException($"Customer already has a ShoppingCart.");

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
