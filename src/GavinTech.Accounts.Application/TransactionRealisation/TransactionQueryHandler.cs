using GavinTech.Accounts.Domain.Primitives;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Application.TransactionRealisation
{
    public class TransactionQuery : IRequest<ICollection<Transaction>>
    {
        public Day? StartDayIncl { get; set; }
        public Day EndDayExcl { get; set; }
        public string? AccountName { get; set; }
    }

    public class TransactionQueryHandler : IRequestHandler<TransactionQuery, ICollection<Transaction>>
    {
        private readonly ITransactionRealiser _realiser;

        public TransactionQueryHandler(ITransactionRealiser realiser) => _realiser = realiser;

        public async Task<ICollection<Transaction>> Handle(TransactionQuery request, CancellationToken ct)
        {
            var lazy = await _realiser.EnumerateAsync(request.StartDayIncl, request.EndDayExcl, request.AccountName,
                ct);
            return lazy.ToList();
        }
    }
}
