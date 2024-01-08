using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Customers.Commands.EditProfile
{
    public class EditProfile : ICommand<CustomerCommandResponse>
    {
        public EditProfileDto? Customer { get; set; }
    }
}
