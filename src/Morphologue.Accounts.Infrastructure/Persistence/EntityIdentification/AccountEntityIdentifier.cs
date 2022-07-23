using System;
using System.Linq.Expressions;
using Morphologue.Accounts.CrossCutting.DependencyInjection;
using Morphologue.Accounts.Domain.Entities;

namespace Morphologue.Accounts.Infrastructure.Persistence.EntityIdentification;

[SingletonService]
internal class AccountEntityIdentifier : IEntityIdentifier<Account>
{
    public string Identify(Account entity) => entity.Name;

    public Expression<Func<Account, bool>> MakePredicate(string id) => entity => entity.Name == id;
}