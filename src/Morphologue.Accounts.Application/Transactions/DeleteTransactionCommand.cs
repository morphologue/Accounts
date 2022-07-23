using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Morphologue.Accounts.Application.Interfaces.Persistence;
using Morphologue.Accounts.Domain.Primitives;

namespace Morphologue.Accounts.Application.Transactions;

public class DeleteTransactionCommand : IRequest, ITransactionDeletionRequest
{
    public string Id { get; init; } = string.Empty;
    public Day Day { get; init; }
    public bool EndRecurrence { get; init; }
}

internal class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
{
    private readonly ITransactionDeleter _deleter;
    private readonly IUnitOfWork _uow;

    public DeleteTransactionCommandHandler(ITransactionDeleter deleter, IUnitOfWork uow)
    {
        _deleter = deleter;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteTransactionCommand request, CancellationToken ct)
    {
        _uow.EnableChangeTracking();
        await _deleter.DeleteAsync(request, ct);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
