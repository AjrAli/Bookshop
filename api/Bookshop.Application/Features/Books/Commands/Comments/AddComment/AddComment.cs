using Bookshop.Application.Contracts.MediatR.Command;

namespace Bookshop.Application.Features.Books.Commands.Comments.AddComment
{
    public class AddComment : ICommand<CommentCommandResponse>
    {
        public CommentRequestDto? Comment { get; set; }
    }
}
