using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomer : ICommand<CreateCustomerResponse>
    {
        public CustomerRequestDto? Customer { get; set; }
    }
}
