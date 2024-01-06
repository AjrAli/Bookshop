using AutoMapper;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Bookshop.Application.Features.ShoppingCarts.Queries.GetShoppingCartDetails
{
    public class GetShoppingCartDetailsHandler : IQueryHandler<GetShoppingCartDetails, GetShoppingCartDetailsResponse>
    {
        private readonly BookshopDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetShoppingCartDetailsHandler(BookshopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GetShoppingCartDetailsResponse> Handle(GetShoppingCartDetails request, CancellationToken cancellationToken)
        {
            var shoppingCartDetails = await _dbContext.ShoppingCarts
                                               .Include(x => x.Customer)
                                                   .ThenInclude(x => x.ShippingAddress)
                                                       .ThenInclude(x => x.LocationPricing)
                                               .Include(x => x.LineItems)
                                                   .ThenInclude(x => x.Book)
                                                       .ThenInclude(x => x.Author)
                                               .Include(x => x.LineItems)
                                                   .ThenInclude(x => x.Book)
                                                       .ThenInclude(x => x.Category)
                                               .Where(x => x.Customer.IdentityUserDataId == request.UserId && x.LineItems.Count() > 0)
                                               .Select(x => _mapper.Map<GetShoppingCartDetailsResponseDto>(x))
                                               .FirstOrDefaultAsync(cancellationToken);
            if (shoppingCartDetails == null)
            {
                throw new NotFoundException($"No {nameof(ShoppingCart)} is found for current user");
            }
            shoppingCartDetails.Total = Math.Round(shoppingCartDetails.SubTotal +
                            ((shoppingCartDetails.SubTotal / 100) * shoppingCartDetails.VatRate) +
                            shoppingCartDetails.ShippingFee, 2);
            return new()
            {
                ShoppingCartDetails = shoppingCartDetails
            };
        }
    }
}
