using System.Collections.Generic;
using System.Linq;

namespace WebApiTemplate.Api.Application.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool SafeAny<T>(this IEnumerable<T> source)
        {
            return source?.Any() ?? false;
        }
    }
}
