using Bookshop.Application.Features.Books;
using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Search.Queries.GetSearchResults
{
    public class GetSearchResultsResponse : BaseResponse
    {
        public List<BookResponseDto>? Books { get; set; }
        public GetSearchResultsResponse() : base()
        {

        }
    }
}
