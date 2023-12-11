using Bookshop.Application.Contracts.MediatR.Query;

namespace Bookshop.Application.Features.ShoppingCarts.Queries.GetShoppingCart
{
    public class GetShoppingCart : IQuery<GetShoppingCartResponse>
    {
        public string? UserId { get; set; }
    }
}
