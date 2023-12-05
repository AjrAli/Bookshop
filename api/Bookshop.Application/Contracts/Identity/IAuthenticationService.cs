using Bookshop.Application.Features.Customer.Commands;
using Bookshop.Application.Features.Customer.Queries.Authenticate;

namespace Bookshop.Application.Contracts.Identity
{
    public interface IIdentityUserService
    {
        Task<AuthenticateQueryResponse> Authenticate(string username, string password);
        Task<CreateCustomerCommandResponse> CreateCustomer(CreateCustomerCommand request, CancellationToken cancellationToken);
    }
}
