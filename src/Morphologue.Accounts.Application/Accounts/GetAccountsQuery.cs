using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Morphologue.Accounts.Application.Interfaces.Persistence;
using Morphologue.Accounts.Domain.Entities;
using Morphologue.Accounts.Domain.Extensions;
using Morphologue.Accounts.Domain.Primitives;

namespace Morphologue.Accounts.Application.Accounts;

public class GetAccountsQuery : IRequest<ICollection<Account>>
{
    public Day? AsAtDay { get; set; }
}

internal class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, ICollection<Account>>
{
    private readonly IRepository<Account> _accountRepo;

    public GetAccountsQueryHandler(IRepository<Account> accountRepo) => _accountRepo = accountRepo;

    public async Task<ICollection<Account>> Handle(GetAccountsQuery query, CancellationToken ct)
    {
        var got = await _accountRepo.GetAsync(ct);
        return !query.AsAtDay.HasValue
            ? got
            : got
                .Where(a => a.GetHierarchicalClosedAfter() is var closedAfter
                    && !closedAfter.HasValue || closedAfter.Value > query.AsAtDay.Value)
                .ToList();
    }
}
