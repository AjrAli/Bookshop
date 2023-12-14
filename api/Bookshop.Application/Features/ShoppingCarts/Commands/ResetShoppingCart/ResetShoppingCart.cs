using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.ShoppingCarts.Commands.ResetShoppingCart
{
    public class ResetShoppingCart : ICommand<ResetShoppingCartResponse>
    {
        public string? UserId { get; set; }
        public bool IsSaveChangesAsyncCalled { get; set; }
    }
}
