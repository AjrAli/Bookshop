using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Orders.Queries.GetOrdersOfCustomer
{
    public class GetOrdersOfCustomerResponse : BaseResponse
    {
        public GetOrdersOfCustomerResponse() : base()
        {

        }
        public List<OrderResponseDto>? Orders { get; set; }
    }
}
