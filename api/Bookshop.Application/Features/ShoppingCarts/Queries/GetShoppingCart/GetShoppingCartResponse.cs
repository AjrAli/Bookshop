using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.ShoppingCarts.Queries.GetShoppingCart
{
    public class GetShoppingCartResponse : BaseResponse
    {
        public GetShoppingCartResponse() : base()
        {

        }
        public ShoppingCartResponseDto? ShoppingCart { get; set; }
    }
}
