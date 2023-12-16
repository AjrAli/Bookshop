using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderResponse : CommandResponse
    {
        public UpdateOrderResponse() : base()
        {

        }
        public OrderResponseDto? Order { get; set; }
    }
}
