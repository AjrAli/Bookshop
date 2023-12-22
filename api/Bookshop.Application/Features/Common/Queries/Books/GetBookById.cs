using Bookshop.Application.Contracts.MediatR.Query;

namespace Bookshop.Application.Features.Common.Queries.Books
{
    public class GetBookById : IQuery<GetByIdResponse>
    {
        public long Id { get; set; }
    }
}
