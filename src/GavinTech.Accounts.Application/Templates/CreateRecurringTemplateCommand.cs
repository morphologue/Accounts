using GavinTech.Accounts.Domain.Entities;
using GavinTech.Accounts.Domain.Primitives;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Application.Templates
{
    public class CreateRecurringTemplateCommand
        : IRequest<CreateRecurringTemplateCommandResponse>, ITemplateCreationRequest
    {
        public string AccountName { get; init; } = string.Empty;
        public Day Day { get; init; }
        public Amount Amount { get; init; }
        public string Description { get; init; } = string.Empty;
        public RecurrenceBasis Basis { get; init; }
        public uint Multiplicand { get; init; }
        public Day? UntilExcl { get; init; }
    }

    public class CreateRecurringTemplateCommandResponse
    {
        public string Id { get; init; } = string.Empty;
    }

    internal class CreateRecurringTemplateCommandHandler
        : IRequestHandler<CreateRecurringTemplateCommand, CreateRecurringTemplateCommandResponse>
    {
        private readonly ITemplateWriter<RecurringTransactionTemplate> _writer;

        public CreateRecurringTemplateCommandHandler(ITemplateWriter<RecurringTransactionTemplate> writer)
        {
            _writer = writer;
        }

        public async Task<CreateRecurringTemplateCommandResponse> Handle(
            CreateRecurringTemplateCommand request,
            CancellationToken ct)
        {
            var id = await _writer.CreateAsync(request, ct, template =>
            {
                template.Basis = request.Basis;
                template.Multiplicand = request.Multiplicand;
                template.UntilExcl = request.UntilExcl;
            });

            return new() { Id = id };
        }
    }
}
