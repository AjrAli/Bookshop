using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using static Bookshop.Domain.Entities.Order;

namespace Bookshop.Application.Features.Orders.Queries.GetOrdersOfCustomer
{
    public class GetOrdersOfCustomerHandler : IQueryHandler<GetOrdersOfCustomer, GetOrdersOfCustomerResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetOrdersOfCustomerHandler(BookshopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GetOrdersOfCustomerResponse> Handle(GetOrdersOfCustomer request, CancellationToken cancellationToken)
        {
            var orders = await _dbContext.Orders
                                               .Include(x => x.Customer)
                                                   .ThenInclude(x => x.ShippingAddress)
                                                       .ThenInclude(x => x.LocationPricing)
                                               .Where(x => x.Customer.IdentityUserDataId == request.UserId &&
                                                           x.StatusOrder != Status.Cancelled)
                                               .Select(x => _mapper.Map<OrderResponseDto>(x))
                                               .ToListAsync(cancellationToken);
            if (orders == null)
            {
                throw new NotFoundException($"No Orders found for current user");
            }
            return new()
            {
                Orders = orders
            };
        }
    }
}
