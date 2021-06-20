using GavinTech.Accounts.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Application.Interfaces.Persistence
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<List<TEntity>> GetAsync(CancellationToken ct, Expression<Func<TEntity, bool>>? predicate = null);
        void Add(TEntity entity);
        void Delete(TEntity entity);
    }
}
