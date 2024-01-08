using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Customers
{
    public class CustomerResponse : BaseResponse
    {
        public CustomerResponse() : base()
        {

        }
        public CustomerResponseDto? Customer { get; set; }
        public string? Token { get; set; }
    }
}
