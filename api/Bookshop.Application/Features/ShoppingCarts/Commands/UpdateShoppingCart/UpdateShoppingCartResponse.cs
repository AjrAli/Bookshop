using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.ShoppingCarts.Commands.UpdateShoppingCart
{
    public class UpdateShoppingCartResponse : BaseResponse
    {
        public UpdateShoppingCartResponse() : base()
        {

        }
        public ShoppingCartResponseDto? ShoppingCart { get; set; }
    }
}
