using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GavinTech.Accounts.Application.DependencyInjection;
using GavinTech.Accounts.Application.Interfaces.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GavinTech.Accounts.Infrastructure.Persistence
{
    [ScopedService]
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;

        public Repository(AccountsDbContext dbContext) =>
            _dbSet = (DbSet<TEntity>)dbContext
                .GetType()
                .GetProperties()
                .Where(p => typeof(DbSet<TEntity>).IsAssignableTo(p.PropertyType))
                .Single()
                .GetValue(dbContext);

        public Task<List<TEntity>> GetAllAsync() => _dbSet.ToListAsync();

        public void Add(TEntity entity) => _dbSet.Add(entity);

        public void Delete(TEntity entity) => _dbSet.Remove(entity);
    }
}
