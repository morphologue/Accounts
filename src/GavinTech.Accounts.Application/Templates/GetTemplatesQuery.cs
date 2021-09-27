using GavinTech.Accounts.Application.Interfaces.Persistence;
using GavinTech.Accounts.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Application.Templates
{
    public class GetTemplatesQuery : IRequest<ICollection<TransactionTemplate>>
    {
    }

    internal class GetTemplatesQueryHandler : IRequestHandler<GetTemplatesQuery, ICollection<TransactionTemplate>>
    {
        private readonly IRepository<TransactionTemplate> _templateRepo;

        public GetTemplatesQueryHandler(IRepository<TransactionTemplate> templateRepo) => _templateRepo = templateRepo;

        public async Task<ICollection<TransactionTemplate>> Handle(GetTemplatesQuery _, CancellationToken ct)
        {
            return await _templateRepo.GetAsync(ct);
        }
    }
}
