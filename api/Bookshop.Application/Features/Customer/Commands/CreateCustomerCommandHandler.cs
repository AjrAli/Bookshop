using Bookshop.Application.Contracts.Identity;
using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Customer.Commands
{
    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, CreateCustomerCommandResponse>
    {
        private readonly IIdentityUserService _identityUserService;

        public CreateCustomerCommandHandler(IIdentityUserService identityUserService)
        {
            _identityUserService = identityUserService;
        }

        public async Task<CreateCustomerCommandResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var response = await _identityUserService.CreateCustomer(request, cancellationToken);
            return response;
        }
    }
}
