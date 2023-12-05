using Bookshop.Application.Contracts.Identity;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;

namespace Bookshop.Application.Features.Customer.Queries.Authenticate
{
    public class AuthenticateQueryHandler : IQueryHandler<AuthenticateQuery, AuthenticateQueryResponse>
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticateQueryHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<AuthenticateQueryResponse> Handle(AuthenticateQuery request, CancellationToken cancellationToken)
        {
            if (request?.Username == null || request?.Password == null)
                throw new BadRequestException($"One of the credentials given is empty");
            var response = await _authenticationService.Authenticate(request.Username, request.Password);
            response.Message = $"User {request.Username} successfully connected";
            return response;
        }
    }
}
