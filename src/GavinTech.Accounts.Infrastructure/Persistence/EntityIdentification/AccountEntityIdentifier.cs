using GavinTech.Accounts.CrossCutting.DependencyInjection;
using GavinTech.Accounts.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace GavinTech.Accounts.Infrastructure.Persistence.EntityIdentification
{
    [SingletonService]
    internal class AccountEntityIdentifier : IEntityIdentifier<Account>
    {
        public string Identify(Account entity) => entity.Name;

        public Expression<Func<Account, bool>> MakePredicate(string id) => entity => entity.Name == id;
    }
}
