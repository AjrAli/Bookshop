using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.ShoppingCarts.Extension;
using Bookshop.Application.Features.ShoppingCarts.Validation;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.ShoppingCarts.Commands.UpdateShoppingCart
{
    public class UpdateShoppingCartHandler : ICommandHandler<UpdateShoppingCart, UpdateShoppingCartResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateShoppingCartHandler(BookshopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<UpdateShoppingCartResponse> Handle(UpdateShoppingCart request, CancellationToken cancellationToken)
        {
            await ValidateRequest(request);
            GroupItemsByBookId(request.ShoppingCart);
            var updatedShoppingCart = await UpdateShoppingCartFromDto(request.ShoppingCart, cancellationToken);
            UpdateShoppingCartInDatabase(request.ShoppingCart, updatedShoppingCart);
            await SaveChangesAsync(request, cancellationToken);
            var shoppingCartUpdated = await updatedShoppingCart.ToMappedShoppingCartDto(_dbContext, _mapper, cancellationToken);
            return new()
            {
                ShoppingCart = shoppingCartUpdated,
                Message = $"ShoppingCart successfully updated with stock availability",
                Details = updatedShoppingCart.GetQuantityMismatchMessage(request.ShoppingCart.Items)
            };
        }
        private async Task SaveChangesAsync(UpdateShoppingCart request, CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            request.IsSaveChangesAsyncCalled = true;
        }
        private void UpdateShoppingCartInDatabase(ShoppingCartRequestDto shoppingCartDto, ShoppingCart shoppingCart)
        {
            LineItemsRemovedByCustomer(shoppingCartDto, shoppingCart);
            shoppingCart?.UpdateShoppingCartTotal(_dbContext);
        }

        private void LineItemsRemovedByCustomer(ShoppingCartRequestDto shoppingCartDto, ShoppingCart shoppingCart)
        {
            if (shoppingCart?.LineItems?.Count > 0)
            {
                var itemsToRemove = ItemsStoredButRemovedFromCustomerRequest(shoppingCartDto, shoppingCart);
                if (itemsToRemove != null)
                {
                    shoppingCart.LineItems = shoppingCart.LineItems.Where(x => !itemsToRemove.Any(y => y == x))?.ToList();
                    _dbContext.LineItems.RemoveRange(itemsToRemove);
                }
            }
        }

        private static List<LineItem>? ItemsStoredButRemovedFromCustomerRequest(ShoppingCartRequestDto shoppingCartDto, ShoppingCart shoppingCart)
        {
            return shoppingCart?.LineItems?.Where(x => (!shoppingCartDto.Items.Any(y => y.BookId == x.BookId)) && x.BookId != 0)?.ToList();
        }

        private void GroupItemsByBookId(ShoppingCartRequestDto shoppingCartDto)
        {
            // Group if same book in multiple items of ShoppingCartDto
            shoppingCartDto.Items = shoppingCartDto.Items?.GroupBy(x => new { x.BookId, x.Id })
                                                         .Select(item => new ShopItemRequestDto
                                                         {
                                                             BookId = item.Key.BookId,
                                                             Id = item.Key.Id,
                                                             Quantity = item.Sum(x => x.Quantity)
                                                         }).Distinct().ToList();
        }
        private async Task<ShoppingCart> UpdateShoppingCartFromDto(ShoppingCartRequestDto shoppingCartDto, CancellationToken cancellationToken)
        {
            var shoppingCartExisting = await GetExistingShoppingCartOfUser(shoppingCartDto, cancellationToken);
            await UpdateShoppingCartItems(shoppingCartDto, shoppingCartExisting, cancellationToken);
            return shoppingCartExisting;
        }


        private async Task<ShoppingCart?> GetExistingShoppingCartOfUser(ShoppingCartRequestDto shoppingCartDto, CancellationToken cancellationToken)
        {
            return await _dbContext.ShoppingCarts
                                   .Include(x => x.Customer)
                                   .Include(x => x.LineItems)
                                        .ThenInclude(x => x.Book)
                                   .FirstOrDefaultAsync(x => x.Customer.IdentityUserDataId == shoppingCartDto.UserId, cancellationToken);
        }

        private async Task UpdateShoppingCartItems(ShoppingCartRequestDto shoppingCartDto, ShoppingCart? shoppingCartExisting, CancellationToken cancellationToken)
        {
            foreach (var item in shoppingCartDto.Items)
            {
                var bookRequested = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == item.BookId, cancellationToken) ?? throw new ValidationException($"BookId: {item.BookId} not found in the database.");
                var itemOnProcess = shoppingCartExisting.GetItemStoredWithBookId(bookRequested.Id);
                shoppingCartExisting.UpdateCartItem(bookRequested, item.Quantity);
                RemoveItemIfOutOfStockOrInvalidQuantityRequested(shoppingCartExisting, itemOnProcess);
            }
        }

        private void RemoveItemIfOutOfStockOrInvalidQuantityRequested(ShoppingCart? shoppingCartExisting, LineItem? itemToBeRemoved)
        {
            if (itemToBeRemoved != null && (shoppingCartExisting.LineItems == null || !shoppingCartExisting.LineItems.Contains(itemToBeRemoved)))
                _dbContext.LineItems.Remove(itemToBeRemoved);
        }

        public async Task ValidateRequest(UpdateShoppingCart request)
        {
            await request.ShoppingCart.ValidateUpdateShoppingCartRequest(_dbContext);
        }
    }
}
