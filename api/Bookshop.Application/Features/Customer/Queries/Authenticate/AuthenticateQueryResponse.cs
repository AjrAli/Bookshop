using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Customer.Queries.Authenticate
{
    public class AuthenticateQueryResponse : BaseResponse
    {
        public AuthenticateQueryResponse() : base()
        {

        }
        public CustomerDto? Customer { get; set; }
        public string? Token { get; set; }
    }
}
