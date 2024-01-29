using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.Common.Responses;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Orders.Queries
{
    public class GetOrderByIdHandler : IQueryHandler<GetOrderById, GetByIdResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetOrderByIdHandler(IMapper mapper, BookshopDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetByIdResponse> Handle(GetOrderById request, CancellationToken cancellationToken)
        {
            var query = _dbContext.Orders.AsQueryable();
            query = query.Include(x => x.Customer)
                            .ThenInclude(x => x.ShippingAddress)
                                .ThenInclude(x => x.LocationPricing)
                         .Include(x => x.LineItems)
                            .ThenInclude(x => x.Book)
                                .ThenInclude(x => x.Author)
                         .Include(x => x.LineItems)
                            .ThenInclude(x => x.Book)
                                .ThenInclude(x => x.Category);
            var entity = await query.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException($"No {typeof(Order)} with Id : {request.Id} not found");
            }

            var sourceType = typeof(Order);
            var targetType = typeof(OrderResponseDto);

            var dto = _mapper.Map(entity, sourceType, targetType);

            return new()
            {
                Dto = dto
            };
        }
    }
}
