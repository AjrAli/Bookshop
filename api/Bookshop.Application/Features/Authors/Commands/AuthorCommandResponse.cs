using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Authors.Commands
{
    public class AuthorCommandResponse : CommandResponse
    {
        public AuthorCommandResponse() : base()
        {

        }
        public AuthorResponseDto? Author { get; set; }
    }
}
