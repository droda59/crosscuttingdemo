using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common.Cache;
using CrossCutting.Common.Models;
using Microsoft.Extensions.Logging;

namespace CrossCutting.Common.Database
{
    public class UglyUserRepository : IRepository<User>
    {
        private static readonly IDictionary<Guid, User> Users;
        
        private readonly ILogger _logger;
        private readonly ICacheManager _cacheManager;

        static UglyUserRepository()
        {
            var usersList = new List<User>
            {
                new User { Id = Guid.NewGuid(), FirstName = "Marc-André", LastName = "Thériault", BirthDate = new DateTime(1998, 04, 27), Employer = Employer.GSOFT },
                new User { Id = Guid.NewGuid(), FirstName = "David", LastName = "Mainville", BirthDate = new DateTime(1996, 02, 28), Employer = Employer.Sharegate },
                new User { Id = Guid.NewGuid(), FirstName = "Mathieu", LastName = "Richard", BirthDate = new DateTime(1979, 06, 13), Employer = Employer.GSOFT },
                new User { Id = Guid.NewGuid(), FirstName = "Éric", LastName = "Routhier", BirthDate = new DateTime(1982, 08, 03), Employer = Employer.GSOFT },
                new User { Id = Guid.NewGuid(), FirstName = "Antoine", LastName = "Bousquet", BirthDate = new DateTime(1986, 01, 15), Employer = Employer.Sharegate },
                new User { Id = Guid.NewGuid(), FirstName = "Alexandre", LastName = "Arpin", BirthDate = new DateTime(1988, 12, 23), Employer = Employer.Sharegate },
                new User { Id = Guid.NewGuid(), FirstName = "Gabriel", LastName = "Gohier-Roy", BirthDate = new DateTime(1984, 10, 14), Employer = Employer.Officevibe },
            };
            
            Users = new ConcurrentDictionary<Guid, User>(usersList.ToDictionary(x => x.Id));
        }

        public UglyUserRepository(ILoggerFactory loggerFactory, ICacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
            this._logger = loggerFactory.CreateLogger("UglyUserRepository");
        }

        public User Get(Guid id)
        {
            this._logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()}: Getting user with id <{id}>");
            
            try
            {
                var cachedValue = this._cacheManager.Get(id);
                if (cachedValue == null)
                {
                    cachedValue = Users[id];
                    this._cacheManager.Add(cachedValue as User);
                }
                
                return cachedValue as User;           
            }
            catch (Exception e)
            {
                this._logger.LogError(e, $"{DateTime.UtcNow.ToLongTimeString()}: !!! Logged Exception: {e.Message}");
                throw;
            }
        }    
        
        public IEnumerable<User> GetAll()
        {
            this._logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()}: Getting all users");
            
            try
            {
                var valuesList = new List<User>();
                foreach (var value in Users.Values)
                {
                    var cachedValue = this._cacheManager.Get(value.Id);
                    if (cachedValue == null)
                    {
                        this._cacheManager.Add(value); 
                    }
                    
                    valuesList.Add(value);
                }
                
                return valuesList.AsEnumerable();               
            }
            catch (Exception e)
            {
                this._logger.LogError(e, $"{DateTime.UtcNow.ToLongTimeString()}: !!! Logged Exception: {e.Message}");
                throw;
            }
        }

        public User Update(Guid id, User data)
        {
            this._logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()}: Updating user with id <{id}>");
            
            try
            {
                Users[id] = data;
                this._cacheManager.Update(id, Users[id]);

                return Users[id];
            }
            catch (Exception e)
            {
                this._logger.LogError(e, $"{DateTime.UtcNow.ToLongTimeString()}: !!! Logged Exception: {e.Message}");
                throw;
            }
        }

        public bool Delete(Guid id)
        {
            this._logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()}: Deleting user with id <{id}>");
            
            try
            {
                var result = Users.Remove(id);
                if (result)
                {
                    this._cacheManager.Remove(id);
                }

                return result;
            }
            catch (Exception e)
            {
                this._logger.LogError(e, $"{DateTime.UtcNow.ToLongTimeString()}: !!! Logged Exception: {e.Message}");
                throw;
            }
        }

        public User Create(User data)
        {
            this._logger.LogInformation($"{DateTime.UtcNow.ToLongTimeString()}: Creating new user");
            
            try
            {
                var newId = Guid.NewGuid();
                data.Id = newId;
                Users.Add(newId, data);
                
                var createdValue = Users[newId];
                this._cacheManager.Add(createdValue);

                return createdValue;
            }
            catch (Exception e)
            {
                this._logger.LogError(e, $"{DateTime.UtcNow.ToLongTimeString()}: !!! Logged Exception: {e.Message}");
                throw;
            }
        }
    }
}