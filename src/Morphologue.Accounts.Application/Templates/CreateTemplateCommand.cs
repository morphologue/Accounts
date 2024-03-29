﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Morphologue.Accounts.Domain.Entities;
using Morphologue.Accounts.Domain.Primitives;

namespace Morphologue.Accounts.Application.Templates;

public class CreateTemplateCommand : IRequest, ITemplateCreationRequest
{
    public string? AccountName { get; init; }
    public Day Day { get; init; }
    public Amount Amount { get; init; }
    public string Description { get; init; } = string.Empty;
}

internal class CreateTemplateCommandHandler : IRequestHandler<CreateTemplateCommand>
{
    private readonly ITemplateWriter<TransactionTemplate> _writer;

    public CreateTemplateCommandHandler(ITemplateWriter<TransactionTemplate> writer) => _writer = writer;

    public async Task<Unit> Handle(CreateTemplateCommand request, CancellationToken ct)
    {
        await _writer.CreateAsync(request, ct);
        return Unit.Value;
    }
}
