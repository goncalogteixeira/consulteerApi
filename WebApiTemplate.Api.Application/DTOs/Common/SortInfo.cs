using System;
using System.Linq.Expressions;

namespace WebApiTemplate.Api.Application.DTOs.Common
{
    public class SortInfo<T> where T : class
    {
        public bool? Ascending { get; }

        public Expression<Func<T, object>> Key { get; }

        public SortInfo(bool? ascending, Expression<Func<T, object>> key)
        {
            Ascending = ascending;
            Key = key;
        }
    }
}
