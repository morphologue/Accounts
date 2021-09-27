using GavinTech.Accounts.Application.Interfaces.Persistence;
using GavinTech.Accounts.Domain.Entities;
using GavinTech.Accounts.Domain.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Application.Templates
{
    public class DeleteTemplateCommand : IRequest
    {
        public string Id { get; init; } = string.Empty;
    }

    internal class DeleteTemplateCommandHandler : IRequestHandler<DeleteTemplateCommand>
    {
        private readonly IRepository<TransactionTemplate> _templateRepo;
        private readonly IUnitOfWork _uow;

        public DeleteTemplateCommandHandler(IRepository<TransactionTemplate> templateRepo, IUnitOfWork uow)
        {
            _templateRepo = templateRepo;
            _uow = uow;
        }

        public async Task<Unit> Handle(DeleteTemplateCommand request, CancellationToken ct)
        {
            _uow.EnableChangeTracking();

            var template = await _templateRepo.GetAsync(request.Id, ct)
                ?? throw new NotFoundException($"Cannot delete non-existent tempate {request.Id}");

            _templateRepo.Delete(template);

            await _uow.SaveChangesAsync(ct);

            return Unit.Value;
        }
    }
}
