using Bookshop.Application.Contracts.MediatR.Query;

namespace Bookshop.Application.Features.Customers.Queries.Authenticate
{
    public class Authenticate : IQuery<CustomerResponse>
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
