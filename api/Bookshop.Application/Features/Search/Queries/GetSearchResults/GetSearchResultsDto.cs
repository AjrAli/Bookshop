using Bookshop.Application.Features.Dto;

namespace Bookshop.Application.Features.Search.Queries.GetSearchResults
{
    public record GetSearchResultsDto : IBaseDto
    {
        public long Id { get; set; }
        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }
    }
}
