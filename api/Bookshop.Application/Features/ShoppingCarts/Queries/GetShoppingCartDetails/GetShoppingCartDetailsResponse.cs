using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.ShoppingCarts.Queries.GetShoppingCartDetails
{
    public class GetShoppingCartDetailsResponse : BaseResponse
    {
        public GetShoppingCartDetailsResponse() : base()
        {

        }
        public GetShoppingCartDetailsResponseDto? ShoppingCartDetails { get; set; }
    }
}
