using GavinTech.Accounts.Domain.Entities;
using GavinTech.Accounts.Domain.Primitives;
using MediatR;

namespace GavinTech.Accounts.Application.Templates;

public class SpliceRecurringTemplateCommand : IRequest, ITemplateSpliceRequest, IRecurringTemplateCreationRequest
{
    public string Id { get; init; } = string.Empty;
    public string? AccountName { get; init; }
    public Day Day { get; init; }
    public bool EndRecurrence => true;
    public Amount Amount { get; init; }
    public string Description { get; init; } = string.Empty;
    public RecurrenceBasis Basis { get; init; }
    public uint Multiplicand { get; init; }
    public Day? UntilExcl { get; init; }
}

internal class SpliceRecurringTemplateCommandHandler
    : RecurringTemplateCreationCommandBase<SpliceRecurringTemplateCommand>
{
    public SpliceRecurringTemplateCommandHandler(ITemplateWriter<RecurringTransactionTemplate> writer)
        : base(writer.SpliceAsync) { }
}
