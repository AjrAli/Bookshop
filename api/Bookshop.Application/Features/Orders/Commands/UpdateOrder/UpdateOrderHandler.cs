using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Domain.Entities;
using Bookshop.Domain.Service;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using static Bookshop.Domain.Entities.Order;

namespace Bookshop.Application.Features.Orders.Commands.UpdateOrder
{
    /// <summary>
    /// Handles the command to update an existing order.
    /// </summary>
    public class UpdateOrderHandler : ICommandHandler<UpdateOrder, UpdateOrderResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the UpdateOrderHandler class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="stockService">The stock service.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public UpdateOrderHandler(
            BookshopDbContext dbContext,
            IStockService stockService,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _stockService = stockService;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the command to update an existing order.
        /// </summary>
        /// <param name="request">The update order request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the response containing the updated order information.</returns>
        public async Task<UpdateOrderResponse> Handle(UpdateOrder request, CancellationToken cancellationToken)
        {
            // Retrieve the full details of the order to update
            var orderToUpdate = await GetFullDetailsOfOrderToUpdate(request, cancellationToken);

            // Ensure that the order exists for the user with the requested items
            EnsureOrderExistsForUserWithItemsRequested(orderToUpdate, request);

            // Rollback stock quantities of removed items
            var itemsToRemove = await RollbackStockQuantitiesOfRemovedItems(request, orderToUpdate);

            // Remove items in the database
            RemoveItemsInDB(request, orderToUpdate, itemsToRemove);

            // Update order with new total or remove the empty order
            var orderMessage = (orderToUpdate?.LineItems.Count > 0) ? UpdateOrderWithNewTotal(orderToUpdate) : RemoveEmptyOrder(ref orderToUpdate);

            // Save changes to the database
            await _dbContext.SaveChangesAsync();

            // Retrieve the last saved and mapped order details
            var orderUpdatedDto = _mapper.Map<OrderResponseDto>(orderToUpdate);

            // Return the response
            return new UpdateOrderResponse
            {
                Success = true,
                Message = orderMessage,
                Order = orderUpdatedDto,
                IsSaveChangesAsyncCalled = true
            };
        }

        /// <summary>
        /// Ensures that the order exists for the user with the requested items.
        /// </summary>
        /// <param name="orderToUpdate">The order to update.</param>
        /// <param name="request">The update order request.</param>
        private void EnsureOrderExistsForUserWithItemsRequested(Order? orderToUpdate, UpdateOrder request)
        {
            if (orderToUpdate == null || !AreAllItemsValidForOrder(request?.Order?.ItemsId, orderToUpdate.LineItems))
            {
                throw new NotFoundException($"No valid {nameof(Order)} found for order ID {request?.Id} or the selected items are not valid for the current user's order.");
            }
        }

        /// <summary>
        /// Checks if all requested items are valid for the order.
        /// </summary>
        /// <param name="requestedItemIds">The requested item IDs.</param>
        /// <param name="orderLineItems">The line items in the order.</param>
        /// <returns>Returns true if all items are valid for the order.</returns>
        private bool AreAllItemsValidForOrder(IList<long>? requestedItemIds, ICollection<LineItem>? orderLineItems)
        {
            if (requestedItemIds == null || orderLineItems == null)
            {
                return false;
            }

            return requestedItemIds.All(requestedItemId => orderLineItems.Any(orderItem => orderItem.Id == requestedItemId));
        }

        /// <summary>
        /// Retrieves the full details of the order to update.
        /// </summary>
        /// <param name="request">The update order request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the order with full details.</returns>
        private async Task<Order?> GetFullDetailsOfOrderToUpdate(UpdateOrder request, CancellationToken cancellationToken)
        {
            return await _dbContext.Orders
                .Include(x => x.Customer)
                .ThenInclude(x => x.ShippingAddress)
                .ThenInclude(x => x.LocationPricing)
                .Include(x => x.LineItems)
                .ThenInclude(x => x.Book)
                .FirstOrDefaultAsync(x => x.Customer.IdentityUserDataId == request.Order.UserId &&
                                           x.Id == request.Id &&
                                           x.StatusOrder == Status.Pending, cancellationToken);
        }

        /// <summary>
        /// Removes the empty order from the database.
        /// </summary>
        /// <param name="orderToUpdate">The order to remove passed by ref keyword for changing state in and outside method</param>
        /// <returns>Returns a message indicating the removal of the empty order.</returns>
        private string RemoveEmptyOrder(ref Order? orderToUpdate)
        {
            _dbContext.Orders.Remove(orderToUpdate);
            var result = $"Order {orderToUpdate.Id} deleted because it was empty";
            orderToUpdate = null;
            return result ;
        }

        /// <summary>
        /// Updates the order with a new total.
        /// </summary>
        /// <param name="orderToUpdate">The order to update.</param>
        /// <returns>Returns a message indicating the successful update of the order.</returns>
        private string UpdateOrderWithNewTotal(Order orderToUpdate)
        {
            orderToUpdate.CalculateTotalOrder();
            _dbContext.Orders.Update(orderToUpdate);
            return $"Order {orderToUpdate.Id} successfully updated";
        }

        /// <summary>
        /// Removes the items in the database.
        /// </summary>
        /// <param name="request">The update order request.</param>
        /// <param name="orderToUpdate">The order to update.</param>
        /// <param name="itemsToRemove">The list of items to remove.</param>
        private void RemoveItemsInDB(UpdateOrder request, Order orderToUpdate, List<LineItem> itemsToRemove)
        {
            orderToUpdate.LineItems = orderToUpdate.LineItems.Where(x => !request.Order.ItemsId.Any(y => y == x.Id)).ToList();
            _dbContext.LineItems.RemoveRange(itemsToRemove);
        }

        /// <summary>
        /// Rolls back stock quantities of removed items.
        /// </summary>
        /// <param name="request">The update order request.</param>
        /// <param name="orderToUpdate">The order to update.</param>
        /// <returns>Returns the list of items with rolled-back stock quantities.</returns>
        private async Task<List<LineItem>> RollbackStockQuantitiesOfRemovedItems(UpdateOrder request, Order orderToUpdate)
        {
            var itemsToRemove = orderToUpdate.LineItems.Where(x => request.Order.ItemsId.Any(y => y == x.Id)).ToList();
            _stockService.RollbackStockQuantities(itemsToRemove);
            _dbContext.Orders.Update(orderToUpdate);
            await _dbContext.SaveChangesAsync();
            return itemsToRemove;
        }

        /// <summary>
        /// Validates the update order request.
        /// </summary>
        /// <param name="request">The update order request.</param>
        /// <returns>Returns a task representing the validation process.</returns>
        public Task ValidateRequest(UpdateOrder request)
        {
            throw new NotImplementedException();
        }
    }
}
