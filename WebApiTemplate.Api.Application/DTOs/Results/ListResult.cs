using AspNetCoreHero.Results;
using System.Collections.Generic;

namespace WebApiTemplate.Api.Application.DTOs.Results
{
    public class ListResult<T> : Result
    {
        public List<T> Data { get; set; }

        public long TotalCount { get; set; }

        #region Constructors

        private ListResult(bool succeeded, List<T> data = default, string message = null, long count = 0)
        {
            Data = data;
            Succeeded = succeeded;
            Message = message;
            TotalCount = count;
        }

        public static ListResult<T> Failure(string message)
        {
            return new ListResult<T>(false, default, message);
        }

        public static ListResult<T> Success(List<T> data)
        {
            return new ListResult<T>(true, data, null, data.Count);
        }

        #endregion
    }
}
