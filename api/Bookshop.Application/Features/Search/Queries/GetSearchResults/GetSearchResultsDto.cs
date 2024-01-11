
namespace Bookshop.Application.Features.Search.Queries.GetSearchResults
{
    public record GetSearchResultsDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string Price { get; set; }
        public string AuthorName { get; set; }
        public string CategoryTitle { get; set; }
        public string Language { get; set; }
    }
}
