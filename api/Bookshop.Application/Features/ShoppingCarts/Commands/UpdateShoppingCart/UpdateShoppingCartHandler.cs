using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.ShoppingCarts.Extension;
using Bookshop.Application.Features.ShoppingCarts.Queries.GetShoppingCart;
using Bookshop.Application.Features.ShoppingCarts.Validation;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.ShoppingCarts.Commands.UpdateShoppingCart
{
    /// <summary>
    /// Handles the command to update an existing shopping cart.
    /// </summary>
    public class UpdateShoppingCartHandler : ICommandHandler<UpdateShoppingCart, UpdateShoppingCartResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the UpdateShoppingCartHandler class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public UpdateShoppingCartHandler(BookshopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the command to update an existing shopping cart.
        /// </summary>
        /// <param name="request">The update shopping cart request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the response containing the updated shopping cart information.</returns>
        public async Task<UpdateShoppingCartResponse> Handle(UpdateShoppingCart request, CancellationToken cancellationToken)
        {
            // Validate the request
            await ValidateRequest(request);

            // Group items by book ID in the shopping cart DTO
            GroupItemsByBookId(request.ShoppingCart);

            // Update the shopping cart from the DTO
            var updatedShoppingCart = await UpdateShoppingCartFromDto(request.ShoppingCart, cancellationToken);

            // Update the shopping cart in the database
            UpdateShoppingCartInDatabase(request.ShoppingCart, updatedShoppingCart);

            // Save changes to the database
            var isSaveChangesAsync = await SaveChangesAsync(request, cancellationToken);

            // Retrieve the last saved and mapped shopping cart details
            var shoppingCartUpdated = _mapper.Map<ShoppingCartResponseDto>(updatedShoppingCart);

            // Return the response
            return new UpdateShoppingCartResponse
            {
                ShoppingCart = shoppingCartUpdated,
                Message = $"ShoppingCart successfully updated with stock availability",
                Details = updatedShoppingCart.GetQuantityMismatchMessage(request.ShoppingCart.Items),
                IsSaveChangesAsyncCalled = isSaveChangesAsync
            };
        }

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        /// <param name="request">The update shopping cart request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns true if save changes operation is successful.</returns>
        private async Task<bool> SaveChangesAsync(UpdateShoppingCart request, CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        /// <summary>
        /// Updates the shopping cart in the database.
        /// </summary>
        /// <param name="shoppingCartDto">The shopping cart DTO.</param>
        /// <param name="shoppingCart">The updated shopping cart.</param>
        private void UpdateShoppingCartInDatabase(ShoppingCartRequestDto shoppingCartDto, ShoppingCart shoppingCart)
        {
            LineItemsRemovedByCustomer(shoppingCartDto, shoppingCart);
            shoppingCart?.UpdateShoppingCartTotal(_dbContext);
        }

        /// <summary>
        /// Removes line items from the shopping cart.
        /// </summary>
        /// <param name="shoppingCartDto">The shopping cart DTO.</param>
        /// <param name="shoppingCart">The shopping cart.</param>
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

        /// <summary>
        /// Retrieves items stored but removed from customer request.
        /// </summary>
        /// <param name="shoppingCartDto">The shopping cart DTO.</param>
        /// <param name="shoppingCart">The shopping cart.</param>
        /// <returns>Returns the list of line items to be removed.</returns>
        private static List<LineItem>? ItemsStoredButRemovedFromCustomerRequest(ShoppingCartRequestDto shoppingCartDto, ShoppingCart shoppingCart)
        {
            return shoppingCart?.LineItems?.Where(x => (!shoppingCartDto.Items.Any(y => y.BookId == x.BookId)) && x.BookId != 0)?.ToList();
        }

        /// <summary>
        /// Groups items by book ID in the shopping cart DTO.
        /// </summary>
        /// <param name="shoppingCartDto">The shopping cart DTO.</param>
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

        /// <summary>
        /// Updates the shopping cart from the DTO.
        /// </summary>
        /// <param name="shoppingCartDto">The shopping cart DTO.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the updated shopping cart.</returns>
        private async Task<ShoppingCart> UpdateShoppingCartFromDto(ShoppingCartRequestDto shoppingCartDto, CancellationToken cancellationToken)
        {
            var shoppingCartExisting = await GetExistingShoppingCartOfUser(shoppingCartDto, cancellationToken);
            await UpdateShoppingCartItems(shoppingCartDto, shoppingCartExisting, cancellationToken);
            return shoppingCartExisting;
        }

        /// <summary>
        /// Retrieves the existing shopping cart of the user.
        /// </summary>
        /// <param name="shoppingCartDto">The shopping cart DTO.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the existing shopping cart of the user.</returns>
        private async Task<ShoppingCart?> GetExistingShoppingCartOfUser(ShoppingCartRequestDto shoppingCartDto, CancellationToken cancellationToken)
        {
            return await _dbContext.ShoppingCarts
                                   .Include(x => x.Customer)
                                   .Include(x => x.LineItems)
                                        .ThenInclude(x => x.Book)
                                   .FirstOrDefaultAsync(x => x.Customer.IdentityUserDataId == shoppingCartDto.UserId, cancellationToken);
        }

        /// <summary>
        /// Updates the shopping cart items.
        /// </summary>
        /// <param name="shoppingCartDto">The shopping cart DTO.</param>
        /// <param name="shoppingCartExisting">The existing shopping cart.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        private async Task UpdateShoppingCartItems(ShoppingCartRequestDto shoppingCartDto, ShoppingCart? shoppingCartExisting, CancellationToken cancellationToken)
        {
            foreach (var item in shoppingCartDto.Items)
            {
                var bookRequested = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == item.BookId, cancellationToken)
                    ?? throw new BadRequestException($"BookId: {item.BookId} not found in the database.");

                var itemOnProcess = shoppingCartExisting.GetItemStoredWithBookId(bookRequested.Id);
                shoppingCartExisting.UpdateCartItem(bookRequested, item.Quantity);
                RemoveItemIfOutOfStockOrInvalidQuantityRequested(shoppingCartExisting, itemOnProcess);
            }
        }

        /// <summary>
        /// Removes an item if it is out of stock or has an invalid quantity requested.
        /// </summary>
        /// <param name="shoppingCartExisting">The existing shopping cart.</param>
        /// <param name="itemToBeRemoved">The item to be removed.</param>
        private void RemoveItemIfOutOfStockOrInvalidQuantityRequested(ShoppingCart? shoppingCartExisting, LineItem? itemToBeRemoved)
        {
            if (itemToBeRemoved != null && (shoppingCartExisting.LineItems == null || !shoppingCartExisting.LineItems.Contains(itemToBeRemoved)))
                _dbContext.LineItems.Remove(itemToBeRemoved);
        }

        /// <summary>
        /// Validates the update shopping cart request.
        /// </summary>
        /// <param name="request">The update shopping cart request.</param>
        /// <returns>Returns a task representing the validation process.</returns>
        public async Task ValidateRequest(UpdateShoppingCart request)
        {
            await request.ShoppingCart.ValidateUpdateShoppingCartRequest(_dbContext);
        }
    }
}
