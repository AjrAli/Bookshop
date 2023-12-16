using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Service;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using static Bookshop.Domain.Entities.Order;

namespace Bookshop.Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderHandler : ICommandHandler<CancelOrder, CancelOrderResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IStockService _stockService;

        public CancelOrderHandler(BookshopDbContext dbContext,
                                  IStockService stockService)
        {
            _dbContext = dbContext;
            _stockService = stockService;
        }

        public async Task<CancelOrderResponse> Handle(CancelOrder request, CancellationToken cancellationToken)
        {
            var orderToCancel = await _dbContext.Orders
                                                .Include(x => x.Customer)
                                                .Include(x => x.LineItems)
                                                    .ThenInclude(x => x.Book)
                                                .FirstOrDefaultAsync(x => x.Customer.IdentityUserDataId == request.UserId &&
                                                                          x.Id == request.Id &&
                                                                          x.StatusOrder == Status.Pending, cancellationToken);
            if (orderToCancel == null)
            {
                throw new NotFoundException($"{nameof(Order)} {request.Id} is not found for current user");
            }
            _stockService.RollbackStockQuantities(orderToCancel.LineItems);
            orderToCancel.StatusOrder = Status.Cancelled;
            _dbContext.Orders.Update(orderToCancel);
            return new()
            {
                Success = true,
                Message = $"Order {orderToCancel.Id} successfully canceled"
            };
        }

        public Task ValidateRequest(CancelOrder request)
        {
            throw new NotImplementedException();
        }
    }
}
