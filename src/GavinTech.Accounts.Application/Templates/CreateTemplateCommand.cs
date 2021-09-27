using GavinTech.Accounts.Domain.Entities;
using GavinTech.Accounts.Domain.Primitives;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Application.Templates
{
    public class CreateTemplateCommand : IRequest<CreateTemplateCommandResponse>, ITemplateCreationRequest
    {
        public string AccountName { get; init; } = string.Empty;
        public Day Day { get; init; }
        public Amount Amount { get; init; }
        public string Description { get; init; } = string.Empty;
    }

    public class CreateTemplateCommandResponse
    {
        public string Id { get; init; } = string.Empty;
    }

    internal class CreateTemplateCommandHandler : IRequestHandler<CreateTemplateCommand, CreateTemplateCommandResponse>
    {
        private readonly ITemplateWriter<TransactionTemplate> _writer;

        public CreateTemplateCommandHandler(ITemplateWriter<TransactionTemplate> writer) => _writer = writer;

        public async Task<CreateTemplateCommandResponse> Handle(CreateTemplateCommand request, CancellationToken ct)
        {
            return new() { Id = await _writer.CreateAsync(request, ct) };
        }
    }
}
