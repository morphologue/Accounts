using System.Collections.Generic;
using GavinTech.Accounts.Domain.Entities;

namespace GavinTech.Accounts.Application.Interfaces.Infrastructure.Persistence
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        IAsyncEnumerable<TEntity> GetAll();
        void Add(TEntity entity);
        void Delete(TEntity entity);
    }
}
