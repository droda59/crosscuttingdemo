using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common.Models;

namespace CrossCutting.Common.Database
{
    public class UserRepository : IRepository<User>
    {
        private static readonly IDictionary<Guid, User> Users;

        static UserRepository()
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

        public User Get(Guid id)
        {
            return Users[id];
        }
        
        public IEnumerable<User> GetAll()
        {
            return Users.Values.AsEnumerable();
        }

        public User Update(Guid id, User data)
        {
            Users[id] = data;
            
            return Users[id];
        }

        public bool Delete(Guid id)
        {
            return Users.Remove(id);
        }

        public User Create(User data)
        {
            var newId = Guid.NewGuid();
            data.Id = newId;

            Users.Add(newId, data);
            return Users[newId];
        }
    }
}