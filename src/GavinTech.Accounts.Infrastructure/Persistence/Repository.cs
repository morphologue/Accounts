using System;
using System.Collections.Generic;
using GavinTech.Accounts.Application.DependencyInjection;
using GavinTech.Accounts.Application.Interfaces.Infrastructure.Persistence;
using GavinTech.Accounts.Domain.Entities;

namespace GavinTech.Accounts.Infrastructure.Persistence
{
    [ScopedService]
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        public IAsyncEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
