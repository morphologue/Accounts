using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Morphologue.Accounts.Application.Interfaces.Persistence;
using Morphologue.Accounts.Domain.Entities;
using Morphologue.Accounts.Domain.Exceptions;

namespace Morphologue.Accounts.Application.Templates;

public class GetTemplateQuery : IRequest<TransactionTemplate>
{
    public string Id { get; set; } = string.Empty;
}

internal class GetTemplateQueryHandler : IRequestHandler<GetTemplateQuery, TransactionTemplate>
{
    private readonly IRepository<TransactionTemplate> _templateRepo;

    public GetTemplateQueryHandler(IRepository<TransactionTemplate> templateRepo) => _templateRepo = templateRepo;

    public async Task<TransactionTemplate> Handle(GetTemplateQuery query, CancellationToken ct)
    {
        return await _templateRepo.GetAsync(query.Id, ct)
            ?? throw new NotFoundException($"Cannot find transaction template {query.Id}");
    }
}
