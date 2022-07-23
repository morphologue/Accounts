using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Morphologue.Accounts.Domain.Entities;
using Morphologue.Accounts.Domain.Exceptions;
using Morphologue.Accounts.Domain.Primitives;

namespace Morphologue.Accounts.Application.Templates;

internal interface IRecurringTemplateCreationRequest : ITemplateCreationRequest
{
    RecurrenceBasis Basis { get; }
    uint Multiplicand { get; }
    Day? UntilExcl { get; }
}

internal abstract class RecurringTemplateCreationCommandBase<T> : IRequestHandler<T>
    where T : IRequest, IRecurringTemplateCreationRequest
{
    protected delegate Task WriterMethod(
        T request,
        CancellationToken ct,
        Func<RecurringTransactionTemplate, Task>? extender);

    private readonly WriterMethod _writerMethod;

    protected RecurringTemplateCreationCommandBase(WriterMethod writerMethod)
    {
        _writerMethod = writerMethod;
    }

    public virtual async Task<Unit> Handle(T request, CancellationToken ct)
    {
        await _writerMethod(request, ct, template => Extend(request, template));
        return Unit.Value;
    }

    private static Task Extend(IRecurringTemplateCreationRequest request, RecurringTransactionTemplate template)
    {
        template.Basis = request.Basis;
        template.Multiplicand = request.Multiplicand > 0
            ? request.Multiplicand
            : throw new BadRequestException($"{nameof(request.Multiplicand)} must be greater than zero");
        template.UntilExcl = request.UntilExcl;
        return Task.CompletedTask;
    }
}
