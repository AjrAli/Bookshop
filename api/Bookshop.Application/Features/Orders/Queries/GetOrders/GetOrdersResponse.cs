using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Orders.Queries.GetOrders
{
    public class GetOrdersResponse : BaseResponse
    {
        public GetOrdersResponse() : base()
        {

        }
        public List<OrderResponseDto>? Orders { get; set; }
    }
}
