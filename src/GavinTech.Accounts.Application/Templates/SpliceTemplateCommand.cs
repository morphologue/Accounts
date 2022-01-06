using GavinTech.Accounts.Domain.Entities;
using GavinTech.Accounts.Domain.Primitives;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Application.Templates;

public class SpliceTemplateCommand : IRequest, ITemplateSpliceRequest
{
    public string Id { get; init; } = string.Empty;
    public string AccountName { get; init; } = string.Empty;
    public Day Day { get; init; }
    public Amount Amount { get; init; }
    public string Description { get; init; } = string.Empty;
}

internal class SpliceTemplateCommandHandler : IRequestHandler<SpliceTemplateCommand>
{
    private readonly ITemplateWriter<TransactionTemplate> _writer;

    public SpliceTemplateCommandHandler(ITemplateWriter<TransactionTemplate> writer) => _writer = writer;

    public async Task<Unit> Handle(SpliceTemplateCommand request, CancellationToken ct)
    {
        await _writer.CreateAsync(request, ct);
        return Unit.Value;
    }
}