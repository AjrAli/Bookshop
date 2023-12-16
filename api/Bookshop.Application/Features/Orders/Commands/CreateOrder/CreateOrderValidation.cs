using FluentValidation;
using static Bookshop.Domain.Entities.Order;

namespace Bookshop.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderValidation : AbstractValidator<CreateOrder>
    {
        public CreateOrderValidation()
        {
            //Order CreditCards validation
            RuleFor(x => x.Order.MethodOfPayment)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.")
                .IsEnumName(typeof(CreditCards))
                .WithMessage("Invalid credit card.");
        }
    }
}
