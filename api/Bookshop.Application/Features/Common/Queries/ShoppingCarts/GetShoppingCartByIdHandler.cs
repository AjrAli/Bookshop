using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Application.Features.ShoppingCarts;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.Common.Queries.ShoppingCarts
{
    public class GetShoppingCartByIdHandler : IQueryHandler<GetShoppingCartById, GetByIdResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetShoppingCartByIdHandler(IMapper mapper, BookshopDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetByIdResponse> Handle(GetShoppingCartById request, CancellationToken cancellationToken)
        {
            var query = _dbContext.ShoppingCarts.AsQueryable();
            query = query.Include(x => x.LineItems)
                                .ThenInclude(x => x.Book)
                                    .ThenInclude(x => x.Author)
                         .Include(x => x.LineItems)
                                .ThenInclude(x => x.Book)
                                    .ThenInclude(x => x.Category)
                         .Include(x => x.Customer);
            var entity = await query.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity == null)
            {
                throw new NotFoundException($"No {typeof(ShoppingCart)} with Id : {request.Id} not found");
            }

            var sourceType = typeof(ShoppingCart);
            var targetType = typeof(ShoppingCartResponseDto);

            var dto = _mapper.Map(entity, sourceType, targetType);

            return new()
            {
                Dto = dto
            };
        }
    }
}
