using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Customers.Commands.EditPassword
{
    public class EditPassword : ICommand<CustomerCommandResponse>
    {
        public EditPasswordDto? Customer { get; set; }
    }
}
