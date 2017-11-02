using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrossCutting.Common.Database;
using CrossCutting.Common.Models;
using Microsoft.Extensions.Logging;

namespace CrossCutting.Common.Logging
{
    public class LoggingRepository<T> : IRepository<T>
        where T : Document
    {
        private readonly IRepository<T> _repository;
        private readonly ILogger _logger;

        public LoggingRepository(IRepository<T> repository, ILoggerFactory loggerFactory)
        {
            this._repository = repository;
            this._logger = loggerFactory.CreateLogger("LoggingRepository");
        }

        public T Get(Guid id)
        {
            var typeName = typeof(T).Name;
            this._logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()}: Getting {typeName} with id <{id}> from ${this._repository.GetType().Name}");
            
            try
            {
                return this._repository.Get(id);
            }
            catch (Exception e)
            {
                this._logger.LogError(e, $"{DateTime.UtcNow.ToLongTimeString()}: !!! Logged Exception: {e.Message}");
                throw;
            }
        }

        public IEnumerable<T> GetAll()
        {
            var typeName = typeof(T).Name;
            this._logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()}: Getting all {typeName} from ${this._repository.GetType().Name}");
            
            try
            {
                return this._repository.GetAll();
            }
            catch (Exception e)
            {
                this._logger.LogError(e, $"{DateTime.UtcNow.ToLongTimeString()}: !!! Logged Exception: {e.Message}");
                throw;
            }
        }

        public T Update(Guid id, T data)
        {
            var typeName = typeof(T).Name;
            this._logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()}: Updating {typeName} with id <{id}>");
            
            try
            {
                return this._repository.Update(id, data);
            }
            catch (Exception e)
            {
                this._logger.LogError(e, $"{DateTime.UtcNow.ToLongTimeString()}: !!! Logged Exception: {e.Message}");
                throw;
            }
        }

        public bool Delete(Guid id)
        {
            var typeName = typeof(T).Name;
            this._logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()}: Deleting {typeName} with id <{id}>");
            
            try
            {
                return this._repository.Delete(id);
            }
            catch (Exception e)
            {
                this._logger.LogError(e, $"{DateTime.UtcNow.ToLongTimeString()}: !!! Logged Exception: {e.Message}");
                throw;
            }
        }

        public T Create(T data)
        {
            var typeName = typeof(T).Name;
            this._logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()}: Creating new {typeName}");
            
            try
            {
                return this._repository.Create(data);
            }
            catch (Exception e)
            {
                this._logger.LogError(e, $"{DateTime.UtcNow.ToLongTimeString()}: !!! Logged Exception: {e.Message}");
                throw;
            }
        }
    }
}