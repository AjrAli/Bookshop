using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Orders.Queries.GetOrderByIdOfCustomer
{
    public class GetOrderByIdOfCustomerResponse : BaseResponse
    {
        public GetOrderByIdOfCustomerResponse() : base()
        {

        }
        public OrderResponseDto? Order { get; set; }
    }
}
