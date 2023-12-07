using Bookshop.Application.Contracts.MediatR.Query;
using System.Linq.Expressions;

namespace Bookshop.Application.Features.Common.Queries.GetById
{
    public class GetById<T> : IQuery<GetByIdResponse<T>> where T : class
    {
        public Dictionary<Expression<Func<T, object>>, List<Expression<Func<object, object>>>>? NavigationPropertyConfigurations { get; set; }
        public object? Id { get; set; }
    }
}
