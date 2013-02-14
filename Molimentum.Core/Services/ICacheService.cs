using System;
using System.Web.Caching;

namespace Molimentum.Services
{
    public interface ICacheService
    {
        object Get(string key);

        void Add(string key, object value, CacheDependency dependencies,
                 DateTime absoluteExpiration, TimeSpan slidingExpiration,
                 CacheItemPriority priority,
                 CacheItemRemovedCallback onRemoveCallback);
    }
}