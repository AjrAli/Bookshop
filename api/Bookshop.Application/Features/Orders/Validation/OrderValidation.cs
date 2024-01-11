using Bookshop.Application.Exceptions;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Orders.Validation
{
    public static class OrderValidation
    {
        public static async Task ValidateOrderRequest(this OrderRequestDto? orderDto, BookshopDbContext context)
        {
            if (!await context.ShoppingCarts.Include(x => x.Customer).AnyAsync(x => x.Customer.IdentityUserDataId == orderDto.UserId))
                throw new BadRequestException($"ShoppingCart of current customer not found in Database");
            if (!await context.ShoppingCarts.Include(x => x.Customer).Include(x => x.LineItems).AnyAsync(x => x.Customer.IdentityUserDataId == orderDto.UserId &&
                                                                                                              x.LineItems.Count > 0))
                throw new BadRequestException($"ShoppingCart of current customer is empty");
        }
    }
}
