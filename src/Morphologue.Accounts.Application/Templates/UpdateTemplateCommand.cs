using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Morphologue.Accounts.Domain.Entities;
using Morphologue.Accounts.Domain.Primitives;
using Morphologue.Accounts.Domain.Utilities;

namespace Morphologue.Accounts.Application.Templates;

public class UpdateTemplateCommand : IRequest, ITemplateUpdateRequest
{
    public string Id { get; init; } = string.Empty;
    public PatchBox<string> AccountName { get; init; } = new() { Value = string.Empty };
    public PatchBox<Day> Day { get; init; }
    public PatchBox<Amount> Amount { get; init; }
    public PatchBox<string> Description { get; init; } = new() { Value = string.Empty };
}

internal class UpdateTemplateCommandHandler : IRequestHandler<UpdateTemplateCommand>
{
    private readonly ITemplateWriter<TransactionTemplate> _writer;

    public UpdateTemplateCommandHandler(ITemplateWriter<TransactionTemplate> writer) => _writer = writer;

    public async Task<Unit> Handle(UpdateTemplateCommand request, CancellationToken ct)
    {
        await _writer.UpdateAsync(request, ct);
        return Unit.Value;
    }
}