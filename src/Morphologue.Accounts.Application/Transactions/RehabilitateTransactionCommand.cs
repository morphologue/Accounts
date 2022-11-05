using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Morphologue.Accounts.Application.Interfaces.Persistence;
using Morphologue.Accounts.Domain.Entities;
using Morphologue.Accounts.Domain.Exceptions;
using Morphologue.Accounts.Domain.Primitives;

namespace Morphologue.Accounts.Application.Transactions;

public class RehabilitateTransactionCommand : IRequest
{
    public string Id { get; init; } = string.Empty;
    public Day Day { get; init; }
}

internal class RehabilitateTransactionCommandHandler : IRequestHandler<RehabilitateTransactionCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IRepository<RecurringTransactionTemplate> _templateRepo;

    public RehabilitateTransactionCommandHandler(
        IUnitOfWork uow,
        IRepository<RecurringTransactionTemplate> templateRepo)
    {
        _uow = uow;
        _templateRepo = templateRepo;
    }

    public async Task<Unit> Handle(RehabilitateTransactionCommand request, CancellationToken ct)
    {
        _uow.EnableChangeTracking();

        var template = await _templateRepo.GetAsync(request.Id, ct)
            ?? throw new NotFoundException(
                $"Cannot rehabilitate transaction of non-existent recurring template {request.Id}");
        if (!template.Tombstones.Remove(request.Day))
        {
            throw new ForbiddenException($"Transaction template {request.Id} is not suppressed on {request.Day}");
        }

        await _uow.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
