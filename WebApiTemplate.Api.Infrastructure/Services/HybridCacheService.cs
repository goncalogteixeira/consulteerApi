using EasyCaching.Core;
using System;
using System.Threading.Tasks;
using WebApiTemplate.Api.Application.Interfaces.Services;

namespace WebApiTemplate.Api.Infrastructure.Services
{
    public class HybridCacheService : ICacheService
    {
        readonly IEasyCachingProvider _hybridCacheProvider;

        public HybridCacheService(IEasyCachingProvider hybridCacheProvider) => _hybridCacheProvider = hybridCacheProvider;

        public async Task<T> GetAsync<T>(string cacheKey)
        {
            var cacheValue = await _hybridCacheProvider.GetAsync<T>(cacheKey);

            if (cacheValue.HasValue && cacheValue.Value != null)
            {
                return cacheValue.Value;
            }

            return default;
        }

        public Task SetAsync<T>(string cacheKey, T cacheValue)
        {
            return SetAsync(cacheKey, cacheValue, TimeSpan.FromDays(30));
        }

        public async Task SetAsync<T>(string cacheKey, T cacheValue, TimeSpan expiration)
        {
            // if is not null
            if (cacheValue != null)
            {
                await _hybridCacheProvider.SetAsync(cacheKey, cacheValue, expiration);
            }
        }

        public Task RemoveAsync(string cacheKey)
        {
            return _hybridCacheProvider.RemoveAsync(cacheKey);
        }

        public Task RemoveByPrefixAsync(string prefix)
        {
            return _hybridCacheProvider.RemoveByPrefixAsync(prefix);
        }
    }
}
