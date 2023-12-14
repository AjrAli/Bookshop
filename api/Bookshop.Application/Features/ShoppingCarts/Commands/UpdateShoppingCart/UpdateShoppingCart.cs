using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.ShoppingCarts.Commands.UpdateShoppingCart
{
    public class UpdateShoppingCart : ICommand<UpdateShoppingCartResponse>
    {
        public ShoppingCartRequestDto? ShoppingCart { get; set; }
    }
}
