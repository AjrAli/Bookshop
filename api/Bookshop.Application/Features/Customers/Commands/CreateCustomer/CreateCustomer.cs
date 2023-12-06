using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomer : ICommand<CreateCustomerResponse>
    {
        public CustomerDto? Customer { get; set; }
        public bool IsSaveChangesAsyncCalled { get; set; }
    }
}
