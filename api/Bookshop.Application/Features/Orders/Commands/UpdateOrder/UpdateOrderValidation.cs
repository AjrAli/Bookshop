using FluentValidation;

namespace Bookshop.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderValidation : AbstractValidator<UpdateOrder>
    {
        public UpdateOrderValidation()
        {
            //Order id
            RuleFor(x => x.Order.Id).NotEmpty().WithMessage("Order empty").GreaterThan(0).WithMessage("Order invalid");
            //ShopItem Ids for Order
            RuleFor(x => x.Order.ItemsId).NotEmpty().WithMessage("Not items found");
            RuleForEach(x => x.Order.ItemsId).NotEmpty().WithMessage("Invalid id found"); ;
        }
    }
}
