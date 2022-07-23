using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Morphologue.Accounts.Application.Interfaces.Persistence;
using Morphologue.Accounts.Domain.Primitives;

namespace Morphologue.Accounts.Application.Transactions;

public class GetTransactionsQuery : IRequest<ICollection<Transaction>>
{
    public Day? StartDayIncl { get; set; }
    public Day EndDayExcl { get; set; }
    public string? AccountName { get; set; }
}

internal class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, ICollection<Transaction>>
{
    private readonly IUnitOfWork _uow;
    private readonly ITransactionRealiser _realiser;

    public GetTransactionsQueryHandler(IUnitOfWork uow, ITransactionRealiser realiser)
    {
        _uow = uow;
        _realiser = realiser;
    }

    public async Task<ICollection<Transaction>> Handle(GetTransactionsQuery request, CancellationToken ct)
    {
        // As shadow properties are stored only in the change tracker, and the identity of transactions is a shadow
        // property, it is necessary to enable change tracking in order to identify transactions.
        _uow.EnableChangeTracking();

        var lazy = await _realiser.RealiseAsync(request.StartDayIncl, request.EndDayExcl, request.AccountName,
            ct);
        return lazy.Reverse().ToList();
    }
}
