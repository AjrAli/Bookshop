using Bookshop.Application.Features.Books;

namespace Bookshop.Api.Utility.FileRequest
{
    public class BookFileRequestDto
    {
        public IFormFile File { get; set; }
        public BookRequestDto Book { get; set; }
    }
}
