using System.Linq.Expressions;

namespace Bookshop.Application.Features.Common.Helpers
{
    /*
     * PredicateHelper: A utility class for building complex dynamic expressions using predicates.
     */
    public static class PredicateHelper
    {
        /// <summary>
        /// Combines two expressions using the logical OR operator.
        /// </summary>
        /// <typeparam name="T">The type of the parameter in the expressions.</typeparam>
        /// <param name="expr1">The first expression to be combined.</param>
        /// <param name="expr2">The second expression to be combined.</param>
        /// <returns>Returns a new expression representing the logical OR of the two input expressions.</returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            // Invoke the second expression with the parameters of the first expression
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());

            // Combine the bodies of the two expressions using the OR operator
            var orExpression = Expression.Or(expr1.Body, invokedExpr);

            // Create and return a new lambda expression representing the combined expression
            return Expression.Lambda<Func<T, bool>>(orExpression, expr1.Parameters);
        }
    }

}
