using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Orders;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Common.Queries.Orders
{
    public class GetAllOrdersHandler : IQueryHandler<GetAllOrders, GetAllResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllOrdersHandler(IMapper mapper, BookshopDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetAllResponse> Handle(GetAllOrders request, CancellationToken cancellationToken)
        {
            var count = await _dbContext.Orders.CountAsync(cancellationToken);

            if (count == 0)
            {
                throw new NotFoundException($"No {typeof(Order)} found");
            }

            var query = _dbContext.Orders.AsQueryable();
            query = query.Include(x => x.LineItems)
                                .ThenInclude(x => x.Book)
                                    .ThenInclude(x => x.Author)
                         .Include(x => x.LineItems)
                                .ThenInclude(x => x.Book)
                                    .ThenInclude(x => x.Category)
                         .Include(x => x.Customer)
                                .ThenInclude(x => x.ShippingAddress)
                                    .ThenInclude(x => x.LocationPricing);
            var sourceType = typeof(Order);
            var targetType = typeof(OrderResponseDto);

            var listDto = await query
                .Select(x => _mapper.Map(x, sourceType, targetType))
                .ToListAsync(cancellationToken: cancellationToken);

            return new GetAllResponse
            {
                ListDto = listDto
            };
        }
    }
}
