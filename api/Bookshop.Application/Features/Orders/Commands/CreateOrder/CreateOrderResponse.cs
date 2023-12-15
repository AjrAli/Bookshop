using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderResponse : CommandResponse
    {
        public CreateOrderResponse() : base()
        {

        }
        public OrderResponseDto? Order { get; set; }
    }
}
