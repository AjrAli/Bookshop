using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.ShoppingCarts.Commands.CreateShoppingCart
{
    public class CreateShoppingCartResponse : BaseResponse
    {
        public CreateShoppingCartResponse() : base()
        {

        }
        public ShoppingCartDto? ShoppingCart { get; set; }
    }
}
