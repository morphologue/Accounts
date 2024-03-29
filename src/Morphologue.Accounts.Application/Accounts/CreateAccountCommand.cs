﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Morphologue.Accounts.Application.Interfaces.Persistence;
using Morphologue.Accounts.Domain.Entities;
using Morphologue.Accounts.Domain.Exceptions;

namespace Morphologue.Accounts.Application.Accounts;

public class CreateAccountCommand : IRequest
{
    public string? Parent { get; init; }
    public string Name { get; init; } = string.Empty;
}

internal class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand>
{
    private readonly IRepository<Account> _accountRepo;
    private readonly IUnitOfWork _uow;

    public CreateAccountCommandHandler(IRepository<Account> accountRepo, IUnitOfWork uow)
    {
        _accountRepo = accountRepo;
        _uow = uow;
    }

    public async Task<Unit> Handle(CreateAccountCommand request, CancellationToken ct)
    {
        _uow.EnableChangeTracking();

        if (string.IsNullOrEmpty(request.Name))
        {
            throw new BadRequestException("The account requires a name");
        }

        var accounts = await _accountRepo.GetAsync(ct);
        if (accounts.Any(a => a.Name == request.Name))
        {
            throw new ForbiddenException($"An account called '{request.Name}' already exists");
        }

        var parent = accounts.FirstOrDefault(request.Parent == null ? a => a.Parent == null : a => a.Name == request.Parent)
            ?? throw new NotFoundException($"Parent account '{request.Parent}' could not be found");

        _accountRepo.Add(new() {
            Parent = parent,
            Name = request.Name
        });
        await _uow.SaveChangesAsync(ct);

        return Unit.Value;
    }
}
