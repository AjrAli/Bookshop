using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Search.Queries.GetSearchResults
{
    public class GetSearchResultsResponse : BaseResponse
    {
        public List<GetSearchResultsDto>? SearchResultsDto { get; set; }
        public GetSearchResultsResponse() : base()
        {

        }
    }
}
