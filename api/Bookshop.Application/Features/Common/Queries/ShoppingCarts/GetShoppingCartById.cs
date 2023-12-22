using Bookshop.Application.Contracts.MediatR.Query;

namespace Bookshop.Application.Features.Common.Queries.ShoppingCarts
{
    public class GetShoppingCartById : IQuery<GetByIdResponse>
    {
        public long Id { get; set; }
    }
}
