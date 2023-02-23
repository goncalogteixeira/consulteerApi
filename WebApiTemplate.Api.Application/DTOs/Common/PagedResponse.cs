using System.Collections.Generic;

namespace WebApiTemplate.Api.Application.DTOs.Common
{
    public class PagedResponse<T>
    {
        public int Count { get; }

        public int PageNumber { get; }

        public int PageSize { get; }

        public string Message { get; }

        public IEnumerable<T> Data { get; }

        public PagedResponse(IEnumerable<T> data, int pageNumber, int pageSize, int count)
        {
            Count = count;
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public PagedResponse(string message) => Message = message;
    }
}
