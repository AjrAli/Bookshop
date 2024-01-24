using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Customers.Commands
{
    public class CustomerCommandResponse : CommandResponse
    {
        public CustomerCommandResponse() : base()
        {

        }
        public CustomerResponseDto? Customer { get; set; }
        public string? Token { get; set; }
    }
}
