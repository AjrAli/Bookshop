using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Features.Common.Queries;

namespace Bookshop.Application.Features.Books.Queries
{
    public class GetBooksByCategory : IQuery<GetAllResponse>
    {
        public long CategoryId { get; set; }
    }
}
