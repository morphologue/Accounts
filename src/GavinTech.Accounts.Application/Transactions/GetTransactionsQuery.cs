using GavinTech.Accounts.Domain.Primitives;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Application.Transactions;

public class GetTransactionsQuery : IRequest<ICollection<Transaction>>
{
    public Day? StartDayIncl { get; set; }
    public Day EndDayExcl { get; set; }
    public string? AccountName { get; set; }
}

internal class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, ICollection<Transaction>>
{
    private readonly ITransactionRealiser _realiser;

    public GetTransactionsQueryHandler(ITransactionRealiser realiser) => _realiser = realiser;

    public async Task<ICollection<Transaction>> Handle(GetTransactionsQuery request, CancellationToken ct)
    {
        var lazy = await _realiser.RealiseAsync(request.StartDayIncl, request.EndDayExcl, request.AccountName,
            ct);
        return lazy.ToList();
    }
}