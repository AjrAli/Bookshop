using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdResponse : BaseResponse
    {
        public GetOrderByIdResponse() : base()
        {

        }
        public OrderResponseDto? Order { get; set; }
    }
}
