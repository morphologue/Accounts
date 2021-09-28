using GavinTech.Accounts.Application.Interfaces.Persistence;
using GavinTech.Accounts.Domain.Primitives;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Application.Transactions
{
    public class DeleteTransactionCommand : IRequest, ITransactionDeletionRequest
    {
        public string Id { get; init; } = string.Empty;
        public Day Day { get; init; }
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
}
