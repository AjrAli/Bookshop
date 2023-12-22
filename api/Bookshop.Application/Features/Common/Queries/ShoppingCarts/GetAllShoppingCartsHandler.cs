using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.ShoppingCarts;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Common.Queries.ShoppingCarts
{
    public class GetAllShoppingCartsHandler : IQueryHandler<GetAllShoppingCarts, GetAllResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAllShoppingCartsHandler(IMapper mapper, BookshopDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetAllResponse> Handle(GetAllShoppingCarts request, CancellationToken cancellationToken)
        {
            var count = await _dbContext.ShoppingCarts.CountAsync(cancellationToken);

            if (count == 0)
            {
                throw new NotFoundException($"No {typeof(ShoppingCart)} found");
            }

            var query = _dbContext.ShoppingCarts.AsQueryable();
            query = query.Include(x => x.LineItems)
                                .ThenInclude(x => x.Book)
                                    .ThenInclude(x => x.Author)
                         .Include(x => x.LineItems)
                                .ThenInclude(x => x.Book)
                                    .ThenInclude(x => x.Category)
                         .Include(x => x.Customer);
            var sourceType = typeof(ShoppingCart);
            var targetType = typeof(ShoppingCartResponseDto);

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
