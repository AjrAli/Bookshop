using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Authors.Commands.CreateAuthor
{
    public class CreateAuthor : ICommand<AuthorCommandResponse>
    {
        public AuthorRequestDto? Author { get; set; }
    }
}
