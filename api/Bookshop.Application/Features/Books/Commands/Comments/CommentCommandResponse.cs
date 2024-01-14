using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Books.Commands.Comments
{
    public class CommentCommandResponse : CommandResponse
    {
        public CommentCommandResponse() : base()
        {

        }
        public CommentResponseDto? Comment { get; set; }
    }
}
