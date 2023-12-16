using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Orders.Extension;
using Bookshop.Application.Features.Service;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using static Bookshop.Domain.Entities.Order;

namespace Bookshop.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderHandler : ICommandHandler<UpdateOrder, UpdateOrderResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;

        public UpdateOrderHandler(BookshopDbContext dbContext,
                                  IStockService stockService,
                                  IMapper mapper)
        {
            _dbContext = dbContext;
            _stockService = stockService;
            _mapper = mapper;
        }

        public async Task<UpdateOrderResponse> Handle(UpdateOrder request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await GetFullDetailsOfOrderToUpdate(request, cancellationToken);
            EnsureOrderExistsForUserWithItemsRequested(orderToUpdate, request);
            var itemsToRemove = await RollbackStockQuantitiesOfRemovedItems(request, orderToUpdate);
            RemoveItemsInDB(request, orderToUpdate, itemsToRemove);
            var orderMessage = string.Empty;
            if (orderToUpdate?.LineItems.Count > 0)
            {
                orderMessage = UpdateOrderWithNewTotal(orderToUpdate);
            }
            else
            {
                orderMessage = RemoveEmptyOrder(orderToUpdate);
                orderToUpdate = null;
            }
            await _dbContext.SaveChangesAsync();
            var orderUpdatedDto = await orderToUpdate.ToMappedOrderDto(_dbContext, _mapper, cancellationToken);
            return new()
            {
                Success = true,
                Message = orderMessage,
                Order = orderUpdatedDto,
                IsSaveChangesAsyncCalled = true
            };
        }
        private void EnsureOrderExistsForUserWithItemsRequested(Order? orderToUpdate, UpdateOrder request)
        {
            if (orderToUpdate == null || !AreAllItemsValidForOrder(request?.Order?.ItemsId, orderToUpdate.LineItems))
            {
                throw new NotFoundException($"No valid {nameof(Order)} found for order ID {request?.Order?.Id} or the selected items are not valid for the current user's order.");
            }
        }

        private bool AreAllItemsValidForOrder(IList<long>? requestedItemIds, ICollection<LineItem>? orderLineItems)
        {
            if (requestedItemIds == null || orderLineItems == null)
            {
                return false;
            }

            return requestedItemIds.All(requestedItemId => orderLineItems.Any(orderItem => orderItem.Id == requestedItemId));
        }
        private async Task<Order?> GetFullDetailsOfOrderToUpdate(UpdateOrder request, CancellationToken cancellationToken)
        {
            return await _dbContext.Orders
                                               .Include(x => x.Customer)
                                                   .ThenInclude(x => x.ShippingAddress)
                                                       .ThenInclude(x => x.LocationPricing)
                                                .Include(x => x.LineItems)
                                                    .ThenInclude(x => x.Book)
                                                .FirstOrDefaultAsync(x => x.Customer.IdentityUserDataId == request.Order.UserId &&
                                                                          x.Id == request.Order.Id &&
                                                                          x.StatusOrder == Status.Pending, cancellationToken);
        }

        private string RemoveEmptyOrder(Order orderToUpdate)
        {
            _dbContext.Orders.Remove(orderToUpdate);
            return $"Order {orderToUpdate.Id} deleted because it was empty";
        }

        private string UpdateOrderWithNewTotal(Order orderToUpdate)
        {
            orderToUpdate.CalculateTotalOrder();
            _dbContext.Orders.Update(orderToUpdate);
            return $"Order {orderToUpdate.Id} successfully updated";
        }

        private void RemoveItemsInDB(UpdateOrder request, Order orderToUpdate, List<LineItem> itemsToRemove)
        {
            orderToUpdate.LineItems = orderToUpdate.LineItems.Where(x => !request.Order.ItemsId.Any(y => y == x.Id)).ToList();
            _dbContext.LineItems.RemoveRange(itemsToRemove);
        }

        private async Task<List<LineItem>> RollbackStockQuantitiesOfRemovedItems(UpdateOrder request, Order orderToUpdate)
        {
            var itemsToRemove = orderToUpdate.LineItems.Where(x => request.Order.ItemsId.Any(y => y == x.Id)).ToList();
            _stockService.RollbackStockQuantities(itemsToRemove);
            _dbContext.Orders.Update(orderToUpdate);
            await _dbContext.SaveChangesAsync();
            return itemsToRemove;
        }

        public Task ValidateRequest(UpdateOrder request)
        {
            throw new NotImplementedException();
        }
    }
}
