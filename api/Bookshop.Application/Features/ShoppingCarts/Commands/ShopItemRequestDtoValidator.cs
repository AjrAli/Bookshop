using FluentValidation;

namespace Bookshop.Application.Features.ShoppingCarts.Commands
{
    public class ShopItemRequestDtoValidator : AbstractValidator<ShopItemRequestDto>
    {
        public ShopItemRequestDtoValidator()
        {
            // Max 100 quantity
            RuleFor(p => p.Quantity)
                    .GreaterThan(0)
                    .WithMessage("{PropertyName} invalid")
                    .LessThanOrEqualTo(100)
                    .WithMessage("{PropertyName} must not exceed 100");
            RuleFor(p => p.BookId)
                    .GreaterThan(0)
                    .WithMessage("{PropertyName} not found");
        }
    }
}
