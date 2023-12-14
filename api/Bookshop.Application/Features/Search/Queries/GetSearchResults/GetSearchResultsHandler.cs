using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Features.Common.Helpers;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using static Bookshop.Domain.Entities.Book;

namespace Bookshop.Application.Features.Search.Queries.GetSearchResults
{
    public class GetSearchResultsHandler : IQueryHandler<GetSearchResults, GetSearchResultsResponse>
    {
        private readonly BookshopDbContext _dbContext;

        public GetSearchResultsHandler(BookshopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetSearchResultsResponse> Handle(GetSearchResults request, CancellationToken cancellationToken)
        {
            var stringWithoutExtraSpaces = Regex.Replace(request.Keyword, @"\s{2,}", " ");
            var keywords = stringWithoutExtraSpaces.Trim().Split();
            var allSearchResults = new HashSet<GetSearchResultsDto>(new GetSearchResultsDtoComparer());

            foreach (var keyword in keywords)
            {
                // Search for books
                var booksResults = await SearchBooks(keyword, cancellationToken);
                allSearchResults.UnionWith(booksResults);
            }

            // Filter results if multiple keywords were provided
            if (keywords.Length > 1)
            {
                allSearchResults = new HashSet<GetSearchResultsDto>(allSearchResults
                    .Where(r => FullKeywordMatch(r, keywords)),
                    new GetSearchResultsDtoComparer());
            }
            // Order results by the number of keyword matches

            return new()
            {
                SearchResultsDto = allSearchResults
                .OrderByDescending(x => NumberOfMatches(x, keywords))
                .ToList()
            };
        }

        private async Task<IEnumerable<GetSearchResultsDto>> SearchBooks(string keyword, CancellationToken cancellationToken)
        {
            var query = _dbContext.Books.Include(b => b.Author).Include(b => b.Category).AsQueryable();
            // Build OR conditions using Expression
            Expression<Func<Book, bool>> predicate = p => false;
            predicate = predicate.Or(x => x.Title.Contains(keyword) ||
                            x.Description.Contains(keyword) ||
                            x.Publisher.Contains(keyword) ||
                            x.Isbn.Contains(keyword) ||
                            x.Price.ToString().Contains(keyword) ||
                            x.PublishDate.ToString().Contains(keyword) ||
                            x.Author.Name.Contains(keyword) ||
                            x.Category.Title.Contains(keyword));
            var languageStrFromKeyword = Enum.GetNames(typeof(Languages)).FirstOrDefault(x => x.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            if (languageStrFromKeyword != null)
                predicate = predicate.Or(x => x.Language == Enum.Parse<Languages>(languageStrFromKeyword));

            // Apply the dynamic OR conditions
            var results = await query.Where(predicate).Select(x => new GetSearchResultsDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description.Substring(0, 50),
                Details = $"{x.Publisher} - {x.Isbn} - {x.PublishDate.ToShortDateString()}",
                Price = x.Price.ToString(),
                AuthorName = x.Author.Name,
                CategoryTitle = x.Category.Title,
                Language = x.Language.ToString()
            }).ToListAsync(cancellationToken);

            return results;
        }
        private static bool FullKeywordMatch(GetSearchResultsDto result, string[] keywords)
        {
            foreach (var keyword in keywords)
            {
                if (result.Title?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true) continue;
                if (result.Description?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true) continue;
                if (result.Details?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true) continue;
                if (result.Price?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true) continue;
                if (result.AuthorName?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true) continue;
                if (result.CategoryTitle?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true) continue;
                if (result.Language?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true) continue;
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
                else if (result.Description?.IndexOf(keyword, 0, StringComparison.OrdinalIgnoreCase) >= 0)
                    match = true;
                else if (result.Details?.IndexOf(keyword, 0, StringComparison.OrdinalIgnoreCase) >= 0)
                    match = true;
                else if (result.Price?.IndexOf(keyword, 0, StringComparison.OrdinalIgnoreCase) >= 0)
                    match = true;
                else if (result.AuthorName?.IndexOf(keyword, 0, StringComparison.OrdinalIgnoreCase) >= 0)
                    match = true;
                else if (result.CategoryTitle?.IndexOf(keyword, 0, StringComparison.OrdinalIgnoreCase) >= 0)
                    match = true;
                else if (result.Language?.IndexOf(keyword, 0, StringComparison.OrdinalIgnoreCase) >= 0)
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

            return x.Id == y.Id;
        }

        public int GetHashCode(GetSearchResultsDto obj)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + obj.Id.GetHashCode();
                return hash;
            }
        }
    }

}
