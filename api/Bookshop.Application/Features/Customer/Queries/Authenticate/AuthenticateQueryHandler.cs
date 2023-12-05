using Bookshop.Application.Contracts.Identity;
using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Exceptions;

namespace Bookshop.Application.Features.Customer.Queries.Authenticate
{
    public class AuthenticateQueryHandler : IQueryHandler<AuthenticateQuery, AuthenticateQueryResponse>
    {
        private readonly IIdentityUserService _identityUserService;

        public AuthenticateQueryHandler(IIdentityUserService identityUserService)
        {
            _identityUserService = identityUserService;
        }

        public async Task<AuthenticateQueryResponse> Handle(AuthenticateQuery request, CancellationToken cancellationToken)
        {
            if (request?.Username == null || request?.Password == null)
                throw new BadRequestException($"One of the credentials given is empty");
            var response = await _identityUserService.Authenticate(request.Username, request.Password);
            response.Message = $"User {request.Username} successfully connected";
            return response;
        }
    }
}
