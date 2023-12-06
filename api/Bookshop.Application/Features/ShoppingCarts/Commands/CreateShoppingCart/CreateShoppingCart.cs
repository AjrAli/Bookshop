using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.ShoppingCarts.Commands.CreateShoppingCart
{
    public class CreateShoppingCart : ICommand<CreateShoppingCartResponse>
    {
        public ShoppingCartDto? ShoppingCart { get; set; }
        public bool IsSaveChangesAsyncCalled { get; set; }
    }
}
