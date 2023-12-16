using AutoMapper;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Orders.Extension
{
    public static class OrderExtension
    {
        public static async Task<OrderResponseDto?> ToMappedOrderDto(this Order? order, BookshopDbContext context, IMapper mapper, CancellationToken cancellationToken)
        {
            if (order == null)
                return null;

            var orderDto = await context.Orders
                                   .Include(x => x.Customer)
                                       .ThenInclude(x => x.ShippingAddress)
                                           .ThenInclude(x => x.LocationPricing)
                                   .Include(x => x.LineItems)
                                       .ThenInclude(x => x.Book)
                                           .ThenInclude(x => x.Author)
                                   .Include(x => x.LineItems)
                                       .ThenInclude(x => x.Book)
                                            .ThenInclude(x => x.Category)
                                   .Where(x => x.CustomerId == order.CustomerId)
                                   .OrderByDescending(x => x.Id)
                                   .Select(x => mapper.Map<OrderResponseDto>(x))
                                   .FirstOrDefaultAsync(cancellationToken);
            orderDto.StatusOrder = order.StatusOrder.ToString();
            orderDto.MethodOfPayment = order.MethodOfPayment.ToString();
            orderDto.DateOrder = order.DateOrder.ToShortDateString();
            return orderDto;
        }
    }
}
