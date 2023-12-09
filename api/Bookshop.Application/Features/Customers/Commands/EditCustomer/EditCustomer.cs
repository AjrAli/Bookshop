using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Customers.Commands.EditCustomer
{
    public class EditCustomer : ICommand<EditCustomerResponse>
    {
        public EditCustomerDto? Customer { get; set; }
        public bool IsSaveChangesAsyncCalled { get; set; }
    }
}
