using FluentValidation;

namespace Bookshop.Application.Features.ShoppingCarts.Commands.UpdateShoppingCart
{
    public class UpdateShoppingCartValidator : AbstractValidator<UpdateShoppingCart>
    {
        public UpdateShoppingCartValidator()
        {
            //ShopItem for ShoppingCart
            RuleForEach(x => x.ShoppingCart.Items).SetValidator(new ShopItemRequestDtoValidator());
        }
    }
}
