using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.ShoppingCarts.Commands.DeleteShoppingCart
{
    public class DeleteShoppingCart : ICommand<DeleteShoppingCartResponse>
    {
        public string? UserId { get; set; }
        public bool IsSaveChangesAsyncCalled { get; set; }
    }
}
