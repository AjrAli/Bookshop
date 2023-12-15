using FluentValidation;

namespace Bookshop.Application.Features.ShoppingCarts.Commands.UpdateShoppingCart
{
    public class UpdateShoppingCartValidator : AbstractValidator<UpdateShoppingCart>
    {
        public UpdateShoppingCartValidator()
        {
            //ShopItem for ShoppingCart
            RuleFor(x => x.ShoppingCart.Items)
                .NotEmpty()
                .WithMessage("Not items found");
            RuleForEach(x => x.ShoppingCart.Items).SetValidator(new ShopItemRequestDtoValidator());
        }
    }
}
