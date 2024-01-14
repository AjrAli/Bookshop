using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Books.Commands.Comments.UpdateComment
{
    public class UpdateComment : ICommand<CommentCommandResponse>
    {
        public CommentRequestDto? Comment { get; set; }
    }
}
