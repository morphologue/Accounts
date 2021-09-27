using GavinTech.Accounts.Domain.Entities;
using GavinTech.Accounts.Domain.Exceptions;
using GavinTech.Accounts.Domain.Primitives;
using GavinTech.Accounts.Domain.Utilities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Application.Templates
{
    public class UpdateRecurringTemplateCommand : IRequest, ITemplateUpdateRequest
    {
        public string Id { get; init; } = string.Empty;
        public PatchBox<string> AccountName { get; init; } = new() { Value = string.Empty };
        public PatchBox<Day> Day { get; init; }
        public PatchBox<Amount> Amount { get; init; }
        public PatchBox<string> Description { get; init; } = new() { Value = string.Empty };
        public PatchBox<RecurrenceBasis> Basis { get; init; }
        public PatchBox<uint> Multiplicand { get; init; }
        public PatchBox<Day?> UntilExcl { get; init; }
        public PatchBox<HashSet<Day>> Tombstones { get; init; } = new() { Value = new HashSet<Day>() };
    }

    internal class UpdateRecurringTemplateCommandHandler : IRequestHandler<UpdateRecurringTemplateCommand>
    {
        private readonly ITemplateWriter<RecurringTransactionTemplate> _writer;

        public UpdateRecurringTemplateCommandHandler(ITemplateWriter<RecurringTransactionTemplate> writer)
        {
            _writer = writer;
        }

        public async Task<Unit> Handle(UpdateRecurringTemplateCommand request, CancellationToken ct)
        {
            await _writer.UpdateAsync(request, ct, template =>
            {
                if (request.Basis.IsSpecified)
                {
                    template.Basis = request.Basis.Value;
                }

                if (request.Multiplicand.IsSpecified)
                {
                    template.Multiplicand = request.Multiplicand.Value;
                }

                if (request.UntilExcl.IsSpecified)
                {
                    template.UntilExcl = request.UntilExcl.Value;
                }

                if (request.Tombstones.IsSpecified)
                {
                    template.Tombstones = request.Tombstones.Value
                        ?? throw new BadRequestException($"{nameof(request.Tombstones)} may not be null");
                }
            });

            return Unit.Value;
        }
    }
}
