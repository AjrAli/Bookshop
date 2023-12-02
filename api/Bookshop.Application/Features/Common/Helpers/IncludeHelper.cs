using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Bookshop.Application.Features.Common.Helpers
{
    public static class IncludeHelper
    {
        public static IQueryable<T> ApplyIncludesAndThenIncludes<T>(this IQueryable<T> query,
            Dictionary<Expression<Func<T, object>>, List<Expression<Func<object, object>>>> listIncludesThenIncludes) where T : class
        {
            foreach (var item in listIncludesThenIncludes)
            {
                if (item.Key != null)
                {
                    query = query.Include(item.Key);
                    if (item.Value != null)
                    {
                        var multipleChildLevels = (IIncludableQueryable<T, object>)query;
                        foreach (var childBelowLevel in item.Value)
                        {
                            multipleChildLevels = multipleChildLevels.ThenInclude(childBelowLevel);
                        }
                        query = multipleChildLevels;
                    }
                }
            }
            return query;
        }
    }
}
