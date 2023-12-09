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
            await ValidateRequest(request);
            var shoppingCartToDelete = await _dbContext.ShoppingCarts
                                                       .Include(x => x.Customer)
                                                       .FirstOrDefaultAsync(x => x.Id == request.ShoppingCartId &&
                                                                                 x.Customer.IdentityUserDataId == request.UserId, cancellationToken);
            if (shoppingCartToDelete == null)
            {
                throw new NotFoundException($"{nameof(ShoppingCart)} : {request.ShoppingCartId} is not found for current user");
            }
            shoppingCartToDelete.RemoveShoppingCartFromCustomer(_dbContext);
            _dbContext.ShoppingCarts.Remove(shoppingCartToDelete);
            return new DeleteShoppingCartResponse
            {
                Success = true,
                Message = $"ShoppingCart {shoppingCartToDelete.Id} successfully deleted"
            };
        }
        public Task ValidateRequest(DeleteShoppingCart request)
        {
            if (request.ShoppingCartId == 0)
                throw new ValidationException($"ShoppingCart id : {request.ShoppingCartId} is invalid.");
            return Task.CompletedTask;
        }
    }
}
