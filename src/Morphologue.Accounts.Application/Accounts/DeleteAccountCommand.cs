using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Morphologue.Accounts.Application.Interfaces.Persistence;
using Morphologue.Accounts.Domain.Entities;
using Morphologue.Accounts.Domain.Exceptions;
using Morphologue.Accounts.Domain.Extensions;

namespace Morphologue.Accounts.Application.Accounts;

public class DeleteAccountCommand : IRequest
{
    public string Name { get; init; } = string.Empty;
}

internal class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand>
{
    private readonly IRepository<Account> _accountRepo;
    private readonly IRepository<TransactionTemplate> _templateRepo;
    private readonly IUnitOfWork _uow;

    public DeleteAccountCommandHandler(
        IRepository<Account> accountRepo,
        IRepository<TransactionTemplate> templateRepo,
        IUnitOfWork uow)
    {
        _accountRepo = accountRepo;
        _templateRepo = templateRepo;
        _uow = uow;
    }

    public async Task<Unit> Handle(DeleteAccountCommand request, CancellationToken ct)
    {
        _uow.EnableChangeTracking();

        var accounts = (await _accountRepo
                .GetAsync(ct))
            .Where(a => a.IsUnderName(request.Name))
            .ToHashSet();

        if (accounts.Count == 0)
        {
            throw new NotFoundException($"Cannot delete non-existent account '{request.Name}'");
        }

        var templates = (await _templateRepo
                .GetAsync(ct))
            .Where(t => accounts.Contains(t.Account));

        _templateRepo.Delete(templates);
        _accountRepo.Delete(accounts);

        if (accounts.Any(a => a.Parent == null))
        {
            // Re-create the root account.
            _accountRepo.Add(new());
        }

        await _uow.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
