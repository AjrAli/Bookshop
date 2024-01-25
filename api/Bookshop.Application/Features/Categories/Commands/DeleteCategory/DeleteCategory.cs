using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategory : ICommand<CommandResponse>
    {
        public long Id { get; set; }
    }
}
