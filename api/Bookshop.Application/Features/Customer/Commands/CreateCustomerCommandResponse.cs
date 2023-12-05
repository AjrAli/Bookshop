using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Customer.Commands
{
    public class CreateCustomerCommandResponse : BaseResponse
    {
        public CreateCustomerCommandResponse() : base()
        {

        }
        public string? Id { get; set; }
        public string? Token { get; set; }
    }
}
