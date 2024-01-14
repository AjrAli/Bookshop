using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Books
{
    public class BookCommandResponse : CommandResponse
    {
        public BookCommandResponse() : base()
        {

        }
        public BookResponseDto? Book { get; set; }
    }
}
