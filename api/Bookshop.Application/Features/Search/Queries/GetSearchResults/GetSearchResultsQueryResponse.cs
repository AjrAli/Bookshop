using Bookshop.Application.Features.Response;

namespace Bookshop.Application.Features.Search.Queries.GetSearchResults
{
    public class GetSearchResultsQueryResponse : BaseResponse
    {
        public List<GetSearchResultsDto>? SearchResultsDto { get; set; }
        public GetSearchResultsQueryResponse() : base()
        {

        }
    }
}
