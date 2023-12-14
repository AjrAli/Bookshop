using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.ShoppingCarts.Extension;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.ShoppingCarts.Commands.ResetShoppingCart
{
    public class ResetShoppingCartHandler : ICommandHandler<ResetShoppingCart, ResetShoppingCartResponse>
    {
        private readonly BookshopDbContext _dbContext;

        public ResetShoppingCartHandler(BookshopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResetShoppingCartResponse> Handle(ResetShoppingCart request, CancellationToken cancellationToken)
        {
            var shoppingCartToReset = await _dbContext.ShoppingCarts
                                                       .Include(x => x.Customer)
                                                       .Include(x => x.LineItems)
                                                       .FirstOrDefaultAsync(x => x.Customer.IdentityUserDataId == request.UserId, cancellationToken);
            if (shoppingCartToReset == null)
            {
                throw new NotFoundException($"No {nameof(ShoppingCart)} is found for current user");
            }
            shoppingCartToReset.RemoveLineItems(_dbContext);
            shoppingCartToReset.UpdateShoppingCartTotal(_dbContext);
            return new()
            {
                Success = true,
                Message = $"All items of the shoppingcart successfully deleted"
            };
        }

        public Task ValidateRequest(ResetShoppingCart request)
        {
            throw new NotImplementedException();
        }
    }
}
