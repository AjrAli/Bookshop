using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using static Bookshop.Domain.Entities.Order;

namespace Bookshop.Application.Features.Orders.Queries.GetOrderByIdOfCustomer
{
    public class GetOrderByIdOfCustomerHandler : IQueryHandler<GetOrderByIdOfCustomer, GetOrderByIdOfCustomerResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetOrderByIdOfCustomerHandler(BookshopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GetOrderByIdOfCustomerResponse> Handle(GetOrderByIdOfCustomer request, CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                                               .Include(x => x.Customer)
                                                   .ThenInclude(x => x.ShippingAddress)
                                                       .ThenInclude(x => x.LocationPricing)
                                               .Include(x => x.LineItems)
                                                   .ThenInclude(x => x.Book)
                                                       .ThenInclude(x => x.Author)
                                               .Include(x => x.LineItems)
                                                   .ThenInclude(x => x.Book)
                                                       .ThenInclude(x => x.Category)
                                               .Where(x => x.Customer.IdentityUserDataId == request.UserId &&
                                                           x.Id == request.Id &&
                                                           x.StatusOrder != Status.Cancelled)
                                               .Select(x => _mapper.Map<OrderResponseDto>(x))
                                               .FirstOrDefaultAsync(cancellationToken);
            if (order == null)
            {
                throw new NotFoundException($"Order : {request.Id} is not found for current user");
            }
            return new()
            {
                Order = order
            };
        }
    }
}
