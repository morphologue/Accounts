using GavinTech.Accounts.Domain.Entities;
using GavinTech.Accounts.Domain.Primitives;
using MediatR;

namespace GavinTech.Accounts.Application.Templates;

public class CreateRecurringTemplateCommand : IRequest, IRecurringTemplateCreationRequest
{
    public string? AccountName { get; init; }
    public Day Day { get; init; }
    public Amount Amount { get; init; }
    public string Description { get; init; } = string.Empty;
    public RecurrenceBasis Basis { get; init; }
    public uint Multiplicand { get; init; }
    public Day? UntilExcl { get; init; }
}

internal class CreateRecurringTemplateCommandHandler
    : RecurringTemplateCreationCommandBase<CreateRecurringTemplateCommand>
{
    public CreateRecurringTemplateCommandHandler(ITemplateWriter<RecurringTransactionTemplate> writer)
        : base(writer.CreateAsync) { }
}
