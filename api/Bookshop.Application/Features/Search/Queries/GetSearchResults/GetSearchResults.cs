using Bookshop.Application.Contracts.MediatR.Query;

namespace Bookshop.Application.Features.Search.Queries.GetSearchResults
{
    public class GetSearchResults : IQuery<GetSearchResultsResponse>
    {
        public string Keyword { get; set; }
    }
}
