using System;
using CrossCutting.Common.Models;

namespace CrossCutting.Common.Cache
{
    public interface ICacheManager
    {
        void Add<T>(T data) where T : Document;

        object Get(Guid id);
        
        void Update<T>(Guid id, T data);

        void Remove(Guid id);
    }
}