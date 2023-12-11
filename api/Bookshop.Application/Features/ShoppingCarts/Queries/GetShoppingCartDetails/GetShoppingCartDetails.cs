using Bookshop.Application.Contracts.MediatR.Query;

namespace Bookshop.Application.Features.ShoppingCarts.Queries.GetShoppingCartDetails
{
    public class GetShoppingCartDetails : IQuery<GetShoppingCartDetailsResponse>
    {
        public string? UserId { get; set; }
    }
}
