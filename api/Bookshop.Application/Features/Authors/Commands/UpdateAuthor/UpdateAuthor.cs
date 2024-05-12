using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthor : ICommand<AuthorCommandResponse>
    {
        public AuthorRequestDto? Author { get; set; }
        public long Id { get; set; }
    }
}
