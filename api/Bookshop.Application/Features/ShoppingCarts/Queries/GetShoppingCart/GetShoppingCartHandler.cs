using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.ShoppingCarts.Queries.GetShoppingCart
{
    public class GetShoppingCartHandler : IQueryHandler<GetShoppingCart, GetShoppingCartResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetShoppingCartHandler(BookshopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GetShoppingCartResponse> Handle(GetShoppingCart request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _dbContext.ShoppingCarts
                                               .Include(x => x.LineItems)
                                                   .ThenInclude(x => x.Book)
                                                       .ThenInclude(x => x.Author)
                                               .Include(x => x.LineItems)
                                                   .ThenInclude(x => x.Book)
                                                       .ThenInclude(x => x.Category)
                                               .Where(x => x.Customer.IdentityUserDataId == request.UserId)
                                               .Select(x => _mapper.Map<ShoppingCartResponseDto>(x))
                                               .FirstOrDefaultAsync(cancellationToken);
            if (shoppingCart == null)
            {
                throw new NotFoundException($"No {nameof(ShoppingCart)} is found for current user");
            }
            return new GetShoppingCartResponse
            {
                ShoppingCart = shoppingCart
            };
        }
    }
}
