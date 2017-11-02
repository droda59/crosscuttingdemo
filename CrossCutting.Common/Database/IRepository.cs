using System;
using System.Collections.Generic;
using CrossCutting.Common.Logging;
using CrossCutting.Common.Models;

namespace CrossCutting.Common.Database
{
    public interface IRepository<T>
        where T : Document
    {
        [LoggingDescription("Getting all users")]
        IEnumerable<T> GetAll();

        [LoggingDescription("Getting user")]
        T Get(Guid id);

        [LoggingDescription("Updating user")]
        T Update(Guid id, T data);

        [LoggingDescription("Deleting user")]
        bool Delete(Guid id);

        [LoggingDescription("Creating new user")]
        T Create(T data);
    }
}