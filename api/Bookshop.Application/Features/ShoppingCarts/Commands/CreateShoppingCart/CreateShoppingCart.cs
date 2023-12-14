using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.ShoppingCarts.Commands.CreateShoppingCart
{
    public class CreateShoppingCart : ICommand<CreateShoppingCartResponse>
    {
        public ShoppingCartRequestDto? ShoppingCart { get; set; }
    }
}
