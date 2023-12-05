using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Customer.Commands
{
    public class CreateCustomerCommand : ICommand<CreateCustomerCommandResponse>
    {
        public CustomerDto? Customer { get; set; }
    }
}
