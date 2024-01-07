using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Features.Common.Queries;

namespace Bookshop.Application.Features.Books.Queries
{
    public class GetBooksByAuthor : IQuery<GetAllResponse>
    {
        public long AuthorId { get; set; }
    }
}
