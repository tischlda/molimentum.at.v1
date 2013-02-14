using System;
using System.Web.Caching;
using Molimentum.Services;

namespace Molimentum.Services
{
    public class WebCacheService : ICacheService
    {
        private readonly Cache _cache;

        public WebCacheService(Cache cache)
        {
            _cache = cache;
        }

        public object Get(string key)
        {
            return _cache.Get(key);
        }

        public void Add(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            _cache.Add(key, value, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
        }
    }
}