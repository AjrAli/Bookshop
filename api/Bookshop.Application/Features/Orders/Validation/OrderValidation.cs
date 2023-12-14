using Bookshop.Application.Exceptions;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using static Bookshop.Domain.Entities.Order;

namespace Bookshop.Application.Features.Orders.Validation
{
    public static class OrderValidation
    {
        public static async Task ValidateOrderRequest(this OrderRequestDto? orderDto, BookshopDbContext context)
        {
            if (orderDto == null)
                throw new ValidationException($"{nameof(orderDto)} is required.");

            if (string.IsNullOrEmpty(orderDto.MethodOfPayment))
                throw new ValidationException("Method of payment required for create an order");

            if (!Enum.TryParse<CreditCards>(orderDto.MethodOfPayment, out _))
                throw new ValidationException("Invalid credit card.");
            if (!await context.ShoppingCarts.Include(x => x.Customer).AnyAsync(x => x.Customer.IdentityUserDataId == orderDto.UserId))
                throw new ValidationException($"ShoppingCart of current customer not found in Database");
            if (!await context.ShoppingCarts.Include(x => x.Customer).Include(x => x.LineItems).AnyAsync(x => x.Customer.IdentityUserDataId == orderDto.UserId && 
                                                                                                              x.LineItems.Count > 0))
                throw new ValidationException($"ShoppingCart of current customer is empty");
        }
    }
}
