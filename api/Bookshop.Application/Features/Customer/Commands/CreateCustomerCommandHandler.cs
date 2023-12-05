using Bookshop.Application.Contracts.Identity;
using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Customer.Commands
{
    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, CreateCustomerCommandResponse>
    {
        private readonly IAuthenticationService _authenticationService;

        public CreateCustomerCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<CreateCustomerCommandResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var response = await _authenticationService.CreateCustomer(request, cancellationToken);
            return response;
        }
    }
}
