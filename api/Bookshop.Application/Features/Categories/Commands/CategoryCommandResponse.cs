using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Categories.Commands
{
    public class CategoryCommandResponse : CommandResponse
    {
        public CategoryCommandResponse() : base()
        {

        }
        public CategoryResponseDto? Category { get; set; }
    }
}
