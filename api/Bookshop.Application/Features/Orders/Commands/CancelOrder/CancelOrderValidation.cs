using FluentValidation;

namespace Bookshop.Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderValidation : AbstractValidator<CancelOrder>
    {
        public CancelOrderValidation()
        {
            //Order id
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Order empty")
                .GreaterThan(0)
                .WithMessage("Order invalid");
        }
    }
}
