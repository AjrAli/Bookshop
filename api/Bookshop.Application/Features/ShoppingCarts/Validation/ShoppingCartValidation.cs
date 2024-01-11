using Bookshop.Application.Exceptions;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.ShoppingCarts.Validation
{
    public static class ShoppingCartValidation
    {
        public static async Task ValidateUpdateShoppingCartRequest(this ShoppingCartRequestDto? shoppingCartDto, BookshopDbContext context)
        {
            if (!await context.ShoppingCarts.Include(x => x.Customer).AnyAsync(x => x.Customer.IdentityUserDataId == shoppingCartDto.UserId))
                throw new BadRequestException($"ShoppingCart of current customer not found in Database");

            foreach (var item in shoppingCartDto.Items)
            {
                if (!await context.Books.AnyAsync(x => x.Id == item.BookId))
                    throw new BadRequestException($"BookId: {item.BookId} not found in the database.");
            }
        }
        public static async Task ValidateCreateShoppingCartRequest(this ShoppingCartRequestDto? shoppingCartDto, BookshopDbContext context)
        {
            if (await context.ShoppingCarts.Include(x => x.Customer).AnyAsync(x => x.Customer.IdentityUserDataId == shoppingCartDto.UserId))
                throw new BadRequestException($"Customer already has a ShoppingCart.");

            foreach (var item in shoppingCartDto.Items)
            {
                if (!await context.Books.AnyAsync(x => x.Id == item.BookId))
                    throw new BadRequestException($"BookId: {item.BookId} not found in the database.");
            }
        }
    }
}
