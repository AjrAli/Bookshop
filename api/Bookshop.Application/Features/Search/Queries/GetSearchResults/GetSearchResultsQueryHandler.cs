using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Bookshop.Application.Features.Search.Queries.GetSearchResults
{
    public class GetSearchResultsQueryHandler : IQueryHandler<GetSearchResultsQuery, GetSearchResultsQueryResponse>
    {
        private readonly BookshopDbContext _dbContext;

        public GetSearchResultsQueryHandler(BookshopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetSearchResultsQueryResponse> Handle(GetSearchResultsQuery request, CancellationToken cancellationToken)
        {
            var stringWithoutExtraSpaces = Regex.Replace(request.Keyword, @"\s{2,}", " ");
            var keywords = stringWithoutExtraSpaces.Trim().Split();
            var allSearchResults = new HashSet<GetSearchResultsDto>(new GetSearchResultsDtoComparer());

            foreach (var keyword in keywords)
            {
                // Search for addresses
                var addresResults = await SearchAddresses(keyword, cancellationToken);
                allSearchResults.UnionWith(addresResults);
            }

            // Filter results if multiple keywords were provided
            if (keywords.Length > 1)
            {
                allSearchResults = new HashSet<GetSearchResultsDto>(allSearchResults
                    .Where(r => FullKeywordMatch(r, keywords)),
                    new GetSearchResultsDtoComparer());
            }
            // Order results by the number of keyword matches

            return new GetSearchResultsQueryResponse
            {
                SearchResultsDto = allSearchResults
                .OrderByDescending(x => NumberOfMatches(x, keywords))
                .ToList(),
                Count = allSearchResults
                .OrderByDescending(x => NumberOfMatches(x, keywords)).Count()
            };
        }

        private async Task<IEnumerable<GetSearchResultsDto>> SearchAddresses(string keyword, CancellationToken cancellationToken)
        {
            var results = await _dbContext.Addresses
                .Where(x => x.Id.ToString().Contains(keyword) ||
                            x.City.Contains(keyword) ||
                            x.Country.Contains(keyword) ||
                            x.PostalCode.Contains(keyword) ||
                            x.State.Contains(keyword) ||
                            x.Street.Contains(keyword))
                .Select(x => new GetSearchResultsDto
                {
                    Id = x.Id,
                    Type = "addresses",
                    Title = x.City,
                    Subtitle = $"{x.Street} - {x.PostalCode} - {x.State}",
                    Description = x.Country
                })
                .ToListAsync(cancellationToken);

            return results;
        }
        private static bool FullKeywordMatch(GetSearchResultsDto result, string[] keywords)
        {
            foreach (var keyword in keywords)
            {
                if (result.Title?.ToUpper().Contains(keyword.ToUpper()) == true) continue;
                if (result.Subtitle?.ToUpper().Contains(keyword.ToUpper()) == true) continue;
                if (result.Description?.ToUpper().Contains(keyword.ToUpper()) == true) continue;
                return false;
            }

            return true;
        }

        private static int NumberOfMatches(GetSearchResultsDto result, IEnumerable<string> keywords)
        {
            var counter = 0;

            foreach (var keyword in keywords)
            {
                var match = false;

                if (result.Title?.IndexOf(keyword, 0, StringComparison.OrdinalIgnoreCase) >= 0)
                    match = true;
                else if (result.Subtitle?.IndexOf(keyword, 0, StringComparison.OrdinalIgnoreCase) >= 0)
                    match = true;
                else if (result.Description?.IndexOf(keyword, 0, StringComparison.OrdinalIgnoreCase) >= 0)
                    match = true;

                if (match)
                    counter++;
            }

            return counter;
        }
    }

    // Custom comparer to handle duplicates in the list
    public class GetSearchResultsDtoComparer : IEqualityComparer<GetSearchResultsDto>
    {
        public bool Equals(GetSearchResultsDto? x, GetSearchResultsDto? y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id && x.Type == y.Type;
        }

        public int GetHashCode(GetSearchResultsDto obj)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + obj.Id.GetHashCode();
                hash = hash * 23 + (obj.Type != null ? obj.Type.GetHashCode() : 0);
                return hash;
            }
        }
    }

}
