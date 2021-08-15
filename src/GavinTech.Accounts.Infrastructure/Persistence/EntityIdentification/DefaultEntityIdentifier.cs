using GavinTech.Accounts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace GavinTech.Accounts.Infrastructure.Persistence.EntityIdentification
{
    internal class DefaultEntityIdentifier<TEntity> : IEntityIdentifier<TEntity>
        where TEntity : class, IEntity
    {
        private readonly AccountsDbContext _dbContext;

        public DefaultEntityIdentifier(AccountsDbContext dbContext) =>
            _dbContext = dbContext;

        public string Identify(TEntity entity) =>
            _dbContext.Entry(entity).Property(Constants.IdColumnName).CurrentValue.ToString() ?? string.Empty;

        public Expression<Func<TEntity, bool>> MakePredicate(string id) =>
            entity => EF.Property<int>(entity, Constants.IdColumnName) == int.Parse(id);
    }
}
