using FluentValidation;

namespace Bookshop.Application.Features.ShoppingCarts.Commands
{
    public class ShopItemRequestDtoValidator : AbstractValidator<ShopItemRequestDto>
    {
        public ShopItemRequestDtoValidator()
        {
            // MAx 100 quantity
            RuleFor(p => p.Quantity)
                    .LessThanOrEqualTo(100)
                    .WithMessage("{PropertyName} must not exceed 100");
        }
    }
}
