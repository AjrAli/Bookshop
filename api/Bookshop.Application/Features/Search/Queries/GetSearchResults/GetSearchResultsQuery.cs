using Bookshop.Application.Contracts.MediatR.Query;

namespace Bookshop.Application.Features.Search.Queries.GetSearchResults
{
    public class GetSearchResultsQuery : IQuery<GetSearchResultsQueryResponse>
    {
        public string Keyword { get; set; }
    }
}
