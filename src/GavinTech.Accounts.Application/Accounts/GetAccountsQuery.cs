using GavinTech.Accounts.Application.Interfaces.Persistence;
using GavinTech.Accounts.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Application.Accounts
{
    public class GetAccountsQuery : IRequest<ICollection<Account>>
    {
    }

    internal class GetAccountsQueryHandler : IRequestHandler<GetAccountsQuery, ICollection<Account>>
    {
        private readonly IRepository<Account> _accountRepo;

        public GetAccountsQueryHandler(IRepository<Account> accountRepo) => _accountRepo = accountRepo;

        public async Task<ICollection<Account>> Handle(GetAccountsQuery _, CancellationToken ct)
        {
            return await _accountRepo.GetAsync(ct);
        }
    }
}
