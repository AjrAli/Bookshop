using Bookshop.Application.Exceptions;
using static Bookshop.Domain.Entities.Order;

namespace Bookshop.Application.Features.Orders.Validation
{
    public static class OrderValidation
    {
        public static Task ValidateOrderRequest(this OrderRequestDto? orderDto)
        {
            if (orderDto == null)
                throw new ValidationException($"{nameof(orderDto)} is required.");

            if (string.IsNullOrEmpty(orderDto.MethodOfPayment))
                throw new ValidationException("Method of payment required for create an order");

            if (!Enum.TryParse<CreditCards>(orderDto.MethodOfPayment, out _))
                throw new ValidationException("Invalid credit card.");
            return Task.CompletedTask;
        }
    }
}
