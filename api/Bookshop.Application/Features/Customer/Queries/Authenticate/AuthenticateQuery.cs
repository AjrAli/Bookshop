using Bookshop.Application.Contracts.MediatR.Query;

namespace Bookshop.Application.Features.Customer.Queries.Authenticate
{
    public class AuthenticateQuery : IQuery<AuthenticateQueryResponse>
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
