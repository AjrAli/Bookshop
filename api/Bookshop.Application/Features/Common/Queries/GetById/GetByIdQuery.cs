using Bookshop.Application.Contracts.MediatR.Query;
using Bookshop.Application.Features.Common.Queries.GetAll;
using System.Linq.Expressions;

namespace Bookshop.Application.Features.Common.Queries.GetById
{
    public class GetByIdQuery<T> : IQuery<GetByIdQueryResponse<T>> where T : class
    {
        public Dictionary<Expression<Func<T, object>>, List<Expression<Func<object, object>>>>? NavigationPropertyConfigurations { get; set; }
        public long? Id { get; set; }
    }
}
