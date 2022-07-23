using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Morphologue.Accounts.Application.Interfaces.Persistence;
using Morphologue.Accounts.Domain.Entities;
using Morphologue.Accounts.Domain.Exceptions;
using Morphologue.Accounts.Domain.Extensions;
using Morphologue.Accounts.Domain.Primitives;
using Morphologue.Accounts.Domain.Utilities;

namespace Morphologue.Accounts.Application.Accounts;

public class UpdateAccountCommand : IRequest
{
    public string? Name { get; init; }
    public PatchBox<string> Parent { get; init; } = new() { Value = string.Empty };
    public PatchBox<string> NewName { get; init; } = new() { Value = string.Empty };
    public PatchBox<Day?> ClosedAfter { get; init; }
}

internal class UpdateAccountCommandHandler : IRequestHandler<UpdateAccountCommand>
{
    private readonly IRepository<Account> _accountRepo;
    private readonly IUnitOfWork _uow;

    public UpdateAccountCommandHandler(IRepository<Account> accountRepo, IUnitOfWork uow)
    {
        _accountRepo = accountRepo;
        _uow = uow;
    }

    public async Task<Unit> Handle(UpdateAccountCommand request, CancellationToken ct)
    {
        _uow.EnableChangeTracking();

        var accounts = await _accountRepo.GetAsync(ct);
        var account = accounts.FirstOrDefault(request.Name == null
                ? (a => a.Parent == null)
                : (a => a.Name == request.Name))
            ?? throw new NotFoundException($"Cannot update non-existent account '{request.Name}'");

        if (request.NewName.IsSpecified)
        {
            UpdateName(accounts, account, request.NewName.Value);
        }

        if (request.Parent.IsSpecified)
        {
            UpdateParent(accounts, account, request.Parent.Value);
        }

        if (request.ClosedAfter.IsSpecified)
        {
            UpdateClosedAfter(account, request.ClosedAfter.Value);
        }

        await _uow.SaveChangesAsync(ct);

        return Unit.Value;
    }

    private static void UpdateName(IReadOnlyCollection<Account> accounts, Account account, string target)
    {
        if (string.IsNullOrEmpty(target))
        {
            throw new BadRequestException($"Account '{account.Name}' requires a name");
        }
        if (target == account.Name)
        {
            throw new BadRequestException($"The name of account '{account.Name}' cannot be updated to itself");
        }
        if (accounts.Any(a => a.Name == target))
        {
            throw new ForbiddenException($"Another account with name '{account.Name}' already exists");
        }

        account.Name = target;
    }

    private void UpdateParent(IReadOnlyCollection<Account> accounts, Account account, string? target)
    {
        if (target == account.Name || (target == null && account.Parent == null))
        {
            throw new BadRequestException($"The parent of account '{account.Name}' cannot be updated to itself");
        }
        var parent = accounts.FirstOrDefault(target == null ? a => a.Parent == null : a => a.Name == target)
            ?? throw new NotFoundException($"Parent account '{target}' cannot be found");

        var subNames = accounts
            .Where(a => a != account && a.IsUnderName(account.Name))
            .Select(a => a.Name);
        if (subNames.Any(s => s == target))
        {
            throw new ForbiddenException($"Circularity detected");
        }

        account.Parent = parent;
    }

    private void UpdateClosedAfter(Account account, Day? target)
    {
        if (target == account.ClosedAfter)
        {
            throw new BadRequestException($"The closure date of account '{account.Name}' cannot be updated to "
                + "itself");
        }

        account.ClosedAfter = target;
    }
}
