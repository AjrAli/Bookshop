using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.ShoppingCarts.Extension;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.ShoppingCarts.Commands.DeleteShoppingCart
{
    public class DeleteShoppingCartHandler : ICommandHandler<DeleteShoppingCart, DeleteShoppingCartResponse>
    {
        private readonly BookshopDbContext _dbContext;

        public DeleteShoppingCartHandler(BookshopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DeleteShoppingCartResponse> Handle(DeleteShoppingCart request, CancellationToken cancellationToken)
        {
            var shoppingCartToDelete = await _dbContext.ShoppingCarts
                                                       .Include(x => x.Customer)
                                                       .Include(x => x.LineItems)
                                                       .FirstOrDefaultAsync(x => x.Customer.IdentityUserDataId == request.UserId, cancellationToken);
            if (shoppingCartToDelete == null)
            {
                throw new NotFoundException($"No {nameof(ShoppingCart)} is found for current user");
            }
            shoppingCartToDelete.RemoveLineItems(_dbContext);
            shoppingCartToDelete.UpdateShoppingCartTotal(_dbContext);
            return new()
            {
                Success = true,
                Message = $"ShoppingCart {shoppingCartToDelete.Id} successfully deleted"
            };
        }

        public Task ValidateRequest(DeleteShoppingCart request)
        {
            throw new NotImplementedException();
        }
    }
}
