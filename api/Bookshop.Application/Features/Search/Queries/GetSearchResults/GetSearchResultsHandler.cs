using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Features.Books;
using Bookshop.Application.Features.Common.Helpers;
using Bookshop.Domain.Entities;
using Bookshop.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading;
using static Bookshop.Domain.Entities.Book;

namespace Bookshop.Application.Features.Search.Queries.GetSearchResults
{
    /// <summary>
    /// GetSearchResultsHandler: Handles the query to retrieve search results for books.
    /// </summary>
    public class GetSearchResultsHandler : IQueryHandler<GetSearchResults, GetSearchResultsResponse>
    {
        private readonly BookshopDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the GetSearchResultsHandler class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public GetSearchResultsHandler(BookshopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Handles the query to retrieve search results for books.
        /// </summary>
        /// <param name="request">The search query request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns the response containing the search results.</returns>
        public async Task<GetSearchResultsResponse> Handle(GetSearchResults request, CancellationToken cancellationToken)
        {
            // Remove extra spaces from the search keyword
            var stringWithoutExtraSpaces = Regex.Replace(request.Keyword, @"\s{2,}", " ");
            var keywords = stringWithoutExtraSpaces.Trim().Split();
            var allSearchResults = new HashSet<BookResponseDto>(new GetBookSearchResultsDtoComparer());

            // Iterate through each keyword and search for books
            foreach (var keyword in keywords)
            {
                var booksResults = await SearchBooks(keyword, cancellationToken);
                allSearchResults.UnionWith(booksResults);
            }

            // Filter results if multiple keywords were provided
            if (keywords.Length > 1)
            {
                allSearchResults = new HashSet<BookResponseDto>(allSearchResults
                    .Where(r => FullKeywordMatch(r, keywords)),
                    new GetBookSearchResultsDtoComparer());
            }

            // Order results by the number of keyword matches
            return new GetSearchResultsResponse
            {
                Books = allSearchResults
                    .OrderByDescending(x => NumberOfMatches(x, keywords))
                    .ToList()
            };
        }

        /// <summary>
        /// Searches for books based on a keyword.
        /// </summary>
        /// <param name="keyword">The search keyword.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns a collection of BookResponseDto.</returns>
        private async Task<IEnumerable<BookResponseDto>> SearchBooks(string keyword, CancellationToken cancellationToken)
        {
            // Create a queryable database view for books
            var query = _dbContext.Books.Include(b => b.Author).Include(b => b.Category).AsQueryable();

            // Build OR conditions using Expression for dynamic search
            Expression<Func<Book, bool>> predicate = p => false;
            predicate = predicate.Or(x => x.Title.Contains(keyword) ||
                            x.Description.Contains(keyword) ||
                            x.Publisher.Contains(keyword) ||
                            x.Isbn.Contains(keyword) ||
                            x.Price.ToString().Contains(keyword) ||
                            x.PublishDate.ToString().Contains(keyword) ||
                            x.Author.Name.Contains(keyword) ||
                            x.Category.Title.Contains(keyword));

            // Extract language from the keyword and add it to the search
            var languageStrFromKeyword = Enum.GetNames(typeof(Languages)).FirstOrDefault(x => x.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            if (languageStrFromKeyword != null)
                predicate = predicate.Or(x => x.Language == Enum.Parse<Languages>(languageStrFromKeyword));

            // Apply the dynamic OR conditions to the query
            var results = await query.Where(predicate).Select(x => new BookResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                ImageUrl = x.ImageUrl,
                Quantity = x.Quantity,
                Description = x.Description.Substring(0, 50),
                Isbn = x.Isbn,
                Price = x.Price,
                AuthorName = x.Author.Name,
                CategoryTitle = x.Category.Title,
                Language = x.Language.ToString()
            }).ToListAsync(cancellationToken);

            return results;
        }

        /// <summary>
        /// Checks if a BookResponseDto fully matches all provided keywords.
        /// </summary>
        /// <param name="result">The BookResponseDto to check.</param>
        /// <param name="keywords">The array of keywords to match.</param>
        /// <returns>Returns true if the BookResponseDto fully matches all keywords.</returns>
        private static bool FullKeywordMatch(BookResponseDto result, string[] keywords)
        {
            foreach (var keyword in keywords)
            {
                if (result.Title?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true) continue;
                if (result.Isbn?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true) continue;
                if (result.Price.ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase) == true) continue;
                if (result.AuthorName?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true) continue;
                if (result.CategoryTitle?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true) continue;
                if (result.Language?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true) continue;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Counts the number of keyword matches in a BookResponseDto.
        /// </summary>
        /// <param name="result">The BookResponseDto to count matches in.</param>
        /// <param name="keywords">The collection of keywords to match.</param>
        /// <returns>Returns the number of keyword matches.</returns>
        private static int NumberOfMatches(BookResponseDto result, IEnumerable<string> keywords)
        {
            var counter = 0;

            foreach (var keyword in keywords)
            {
                var match = false;

                if (result.Title?.IndexOf(keyword, 0, StringComparison.OrdinalIgnoreCase) >= 0)
                    match = true;
                else if (result.Isbn?.IndexOf(keyword, 0, StringComparison.OrdinalIgnoreCase) >= 0)
                    match = true;
                else if (result.Price.ToString().IndexOf(keyword, 0, StringComparison.OrdinalIgnoreCase) >= 0)
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

        /// <summary>
        /// Custom comparer to handle duplicates in the list.
        /// </summary>
        private class GetBookSearchResultsDtoComparer : IEqualityComparer<BookResponseDto>
        {
            /// <summary>
            /// Determines whether two BookResponseDto instances are equal.
            /// </summary>
            public bool Equals(BookResponseDto? x, BookResponseDto? y)
            {
                if (x == null && y == null)
                    return true;
                if (x == null || y == null)
                    return false;

                return x.Id == y.Id;
            }

            /// <summary>
            /// Gets the hash code for a BookResponseDto instance.
            /// </summary>
            public int GetHashCode(BookResponseDto obj)
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
}
