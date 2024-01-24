using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Features.Common.Responses;

namespace Bookshop.Application.Features.Authors.Queries
{
    public class GetAuthorById : IQuery<GetByIdResponse>
    {
        public long Id { get; set; }
    }
}
