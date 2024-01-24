using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Features.Common.Responses;

namespace Bookshop.Application.Features.Books.Queries
{
    public class GetBookById : IQuery<GetByIdResponse>
    {
        public long Id { get; set; }
    }
}
