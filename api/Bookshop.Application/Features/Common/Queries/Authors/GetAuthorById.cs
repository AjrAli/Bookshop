using Bookshop.Application.Contracts.MediatR.Query;

namespace Bookshop.Application.Features.Common.Queries.Authors
{
    public class GetAuthorById : IQuery<GetByIdResponse>
    {
        public long Id { get; set; }
    }
}
