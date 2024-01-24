using Bookshop.Application.Contracts.MediatR.Command;
using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthor : ICommand<CommandResponse>
    {
        public long Id { get; set; }
    }
}
