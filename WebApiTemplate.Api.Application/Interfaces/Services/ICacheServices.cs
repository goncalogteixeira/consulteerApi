using System;
using System.Threading.Tasks;

namespace WebApiTemplate.Api.Application.Interfaces.Services
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string cacheKey);
        Task SetAsync<T>(string cacheKey, T cacheValue);
        Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration);
        Task RemoveAsync(string cacheKey);
        Task RemoveByPrefixAsync(string prefix);
    }
}
