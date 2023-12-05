using AutoMapper;
using Bookshop.Application.Exceptions;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;

namespace Bookshop.Application.Features.Service
{
    public class OrderProcessingService
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public OrderProcessingService(BookshopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task ProcessOrder(Order order)
        {
            // Additional order processing logic can go here

            UpdateStockQuantities(order);

            // Update changes 
            await _dbContext.Orders.AddAsync(order);
            foreach (var lineItem in order.LineItems)
            {
                if (lineItem.Book != null)
                {
                    _dbContext.Books.Update(lineItem.Book);
                }
            }
        }

        private void UpdateStockQuantities(Order order)
        {
            foreach (var lineItem in order.LineItems)
            {
                if (lineItem.Book != null)
                {
                    lineItem.Book.Quantity -= lineItem.Quantity;

                    if (lineItem.Book.Quantity < 0)
                    {
                        // Rollback changes if stock is insufficient
                        RollbackStockUpdate(order);
                        throw new BadRequestException("Insufficient quantity!");
                    }
                }
            }
        }

        private void RollbackStockUpdate(Order order)
        {
            foreach (var lineItem in order.LineItems)
            {
                if (lineItem.Book != null)
                {
                    lineItem.Book.Quantity += lineItem.Quantity;
                }
            }
        }
    }
}
