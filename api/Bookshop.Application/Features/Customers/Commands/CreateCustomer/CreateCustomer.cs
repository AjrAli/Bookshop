using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomer : ICommand<CustomerCommandResponse>
    {
        public CustomerRequestDto? Customer { get; set; }
    }
}
