using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Features.Common.Responses;

namespace Bookshop.Application.Features.Books.Queries
{
    public class GetAllBooks : IQuery<GetAllResponse>
    {
    }
}
