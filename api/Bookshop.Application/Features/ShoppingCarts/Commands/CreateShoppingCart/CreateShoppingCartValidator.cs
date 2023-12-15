using FluentValidation;

namespace Bookshop.Application.Features.ShoppingCarts.Commands.CreateShoppingCart
{
    public class CreateShoppingCartValidator : AbstractValidator<CreateShoppingCart>
    {
        public CreateShoppingCartValidator()
        {
            //ShopItem for ShoppingCart
            RuleForEach(x => x.ShoppingCart.Items).SetValidator(new ShopItemRequestDtoValidator());
        }
    }
}
