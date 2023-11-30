using Bookshop.Application.Contracts.MediatR.Query;

namespace Bookshop.Application.Features.Common.Queries.GetAll
{
    public class GetAllQuery<T> : IQuery<GetAllQueryResponse<T>> where T : class
    {
    }
}
