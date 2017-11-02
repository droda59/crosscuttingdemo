using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrossCutting.Common.Database;
using CrossCutting.Common.Models;

namespace CrossCutting.Common.Cache
{
    public class CacheRepository<T> : IRepository<T>
        where T : Document
    {
        private readonly IRepository<T> _repository;
        private readonly ICacheManager _cacheManager;

        public CacheRepository(IRepository<T> repository, ICacheManager cacheManager)
        {
            this._repository = repository;
            this._cacheManager = cacheManager;
        }

        public T Get(Guid id)
        {
            var cachedValue = this._cacheManager.Get(id);
            if (cachedValue == null)
            {
                cachedValue = this._repository.Get(id);
                this._cacheManager.Add((T) cachedValue);
            }

            return cachedValue as T;
        }

        public IEnumerable<T> GetAll()
        {
            var values = this._repository.GetAll();
            var valuesList = new List<T>();
            foreach (var value in values)
            {
                var cachedValue = this._cacheManager.Get(value.Id);
                if (cachedValue == null)
                {
                    this._cacheManager.Add(value); 
                }
                    
                valuesList.Add(value);
            }

            return valuesList;
        }

        public T Update(Guid id, T data)
        {
            var updatedValueFromRepo = this._repository.Update(id, data);
            this._cacheManager.Update(id, updatedValueFromRepo);

            return updatedValueFromRepo;
        }

        public bool Delete(Guid id)
        {
            var result = this._repository.Delete(id);
            if (result)
            {
                this._cacheManager.Remove(id);
            }

            return result;
        }

        public T Create(T data)
        {
            var createdValue = this._repository.Create(data);
            this._cacheManager.Add(createdValue);

            return createdValue;
        }
    }
}