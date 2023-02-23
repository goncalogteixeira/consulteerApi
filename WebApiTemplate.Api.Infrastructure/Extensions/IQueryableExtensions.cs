using System;
using System.Linq;
using System.Linq.Expressions;

namespace WebApiTemplate.Api.Infrastructure.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> SortBy<T, TKey>(this IQueryable<T> source, bool? ascending, Expression<Func<T, TKey>> keySelector)
        {
            if (source == null || !ascending.HasValue)
            {
                return source;
            }

            if (source.IsOrdered())
            {
                IOrderedQueryable<T> orderedQuery = source as IOrderedQueryable<T>;

                return ascending.Value
                    ? orderedQuery.ThenBy(keySelector)
                    : orderedQuery.ThenByDescending(keySelector);
            }
            else
            {
                return ascending.Value
                    ? source.OrderBy(keySelector)
                    : source.OrderByDescending(keySelector);
            }
        }

        public static bool IsOrdered<T>(this IQueryable<T> queryable)
        {
            if (queryable == null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            return queryable.Expression.Type == typeof(IOrderedQueryable<T>);
        }
    }
}
