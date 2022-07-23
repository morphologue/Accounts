using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Morphologue.Accounts.Application.Interfaces.Persistence;
using Morphologue.Accounts.CrossCutting.DependencyInjection;
using Morphologue.Accounts.Domain.Entities;
using Morphologue.Accounts.Domain.Exceptions;
using Morphologue.Accounts.Domain.Primitives;

namespace Morphologue.Accounts.Application.Transactions;

/// <summary>Must be used within a unit of work</summary>
internal interface ITransactionDeleter
{
    Task DeleteAsync(ITransactionDeletionRequest request, CancellationToken ct);
}

internal interface ITransactionDeletionRequest
{
    string Id { get; }
    Day Day { get; }
    bool EndRecurrence { get; }
}

[ScopedService]
internal class TransactionDeleter : ITransactionDeleter
{
    private readonly IRepository<RecurringTransactionTemplate> _templateRepo;
    private readonly ITransactionRealiser _realiser;

    public TransactionDeleter(
        IRepository<RecurringTransactionTemplate> templateRepo,
        ITransactionRealiser realiser)
    {
        _templateRepo = templateRepo;
        _realiser = realiser;
    }

    public async Task DeleteAsync(ITransactionDeletionRequest request, CancellationToken ct)
    {
        var template = await _templateRepo.GetAsync(request.Id, ct)
            ?? throw new NotFoundException(
                $"Cannot suppress transaction of non-existent recurring template {request.Id}");

        var realisation = await _realiser.RealiseAsync(request.Day, request.Day + 1, null, ct);
        if (realisation.All(t => t.TemplateId != request.Id))
        {
            throw new BadRequestException($"Cannot suppress transaction of recurring template {request.Id} which "
                + $"is not realised on {request.Day}");
        }

        template.Tombstones.Add(request.Day);

        if (request.EndRecurrence)
        {
            template.UntilExcl = request.Day;
        }
    }
}
