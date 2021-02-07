using System.Collections.Generic;

namespace GavinTech.Accounts.Application.Interfaces.Infrastructure.Persistence
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IAsyncEnumerable<TEntity> GetAll();
        void Add(TEntity entity);
        void Delete(TEntity entity);
    }
}
