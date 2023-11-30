using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Features.Common.Queries.GetAll;

namespace Bookshop.Application.Features.Common.Queries.GetById
{
    public class GetByIdQuery<T> : IQuery<GetByIdQueryResponse<T>> where T : class
    {
        public long? Id { get; set; }
    }
}
