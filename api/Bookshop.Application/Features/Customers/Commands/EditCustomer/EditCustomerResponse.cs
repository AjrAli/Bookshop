using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Customers.Commands.EditCustomer
{
    public class EditCustomerResponse : BaseResponse
    {
        public EditCustomerResponse() : base()
        {

        }
        public CustomerResponseDto? Customer { get; set; }
        public string? Token { get; set; }
    }
}
