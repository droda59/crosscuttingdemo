using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CrossCutting.Common.Models;

namespace CrossCutting.Common.Cache
{
    public class CacheManager : ICacheManager
    {
        private readonly IDictionary<Guid, object> _cache = new ConcurrentDictionary<Guid, object>();
        
        public void Add<T>(T data)
            where T : Document
        {
            if (!this._cache.ContainsKey(data.Id))
            {
                this._cache.Add(data.Id, data);
            }
        }

        public object Get(Guid id)
        {
            return !this._cache.TryGetValue(id, out var data) ? null : data;
        }

        public void Update<T>(Guid id, T data)
        {
            this._cache[id] = data;
        }

        public void Remove(Guid id)
        {
            this._cache.Remove(id);
        }
    }
}