using GavinTech.Accounts.Application.Interfaces.Persistence;
using GavinTech.Accounts.Application.Transactions;
using GavinTech.Accounts.Domain.Entities;
using GavinTech.Accounts.Domain.Exceptions;
using GavinTech.Accounts.Domain.Primitives;
using GavinTech.Accounts.Domain.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Application.Templates
{
    internal interface ITemplateWriter<T>
        where T : TransactionTemplate, new()
    {
        Task CreateAsync(ITemplateCreationRequest request, CancellationToken ct, Func<T, Task>? extender = null);
        Task UpdateAsync(ITemplateUpdateRequest request, CancellationToken ct, Func<T, Task>? extender = null);
        Task SpliceAsync(ITemplateSpliceRequest request, CancellationToken ct, Func<T, Task>? extender = null);
    }

    internal interface ITemplateCreationRequest
    {
        string AccountName { get; }
        Day Day { get; }
        Amount Amount { get; }
        string Description { get; }
    }

    internal interface ITemplateUpdateRequest
    {
        string Id { get; }
        PatchBox<string> AccountName { get; }
        PatchBox<Amount> Amount { get; }
        PatchBox<Day> Day { get; }
        PatchBox<string> Description { get; }
    }

    internal interface ITemplateSpliceRequest : ITransactionDeletionRequest, ITemplateCreationRequest
    {
    }

    internal class TemplateWriter<T> : ITemplateWriter<T>
        where T : TransactionTemplate, new()
    {
        private readonly IRepository<Account> _accountRepo;
        private readonly IRepository<T> _templateRepo;
        private readonly IUnitOfWork _uow;
        private readonly ITransactionDeleter _deleter;

        public TemplateWriter(
            IRepository<Account> accountRepo,
            IRepository<T> templateRepo,
            IUnitOfWork uow,
            ITransactionDeleter deleter)
        {
            _accountRepo = accountRepo;
            _templateRepo = templateRepo;
            _uow = uow;
            _deleter = deleter;
        }

        public async Task CreateAsync(
            ITemplateCreationRequest request,
            CancellationToken ct,
            Func<T, Task>? extender = null)
        {
            _uow.EnableChangeTracking();

            if (request.Description == null)
            {
                throw new BadRequestException($"{nameof(request.Description)} cannot be null");
            }
            var account = await _accountRepo.GetAsync(request.AccountName, ct)
                ?? throw new NotFoundException($"Cannot find account '{request.AccountName}'");

            var creature = new T
            {
                Day = request.Day,
                Amount = request.Amount,
                Description = request.Description,
                Account = account
            };
            if (extender != null)
            {
                await extender(creature);
            }
            _templateRepo.Add(creature);

            await _uow.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(
            ITemplateUpdateRequest request,
            CancellationToken ct,
            Func<T, Task>? extender = null)
        {
            _uow.EnableChangeTracking();

            var template = await _templateRepo.GetAsync(request.Id, ct)
                ?? throw new NotFoundException($"Cannot update non-existent template {request.Id}");

            if (request.AccountName.IsSpecified)
            {
                var account = await _accountRepo.GetAsync(request.AccountName.Value, ct)
                    ?? throw new NotFoundException($"Cannot find account '{request.AccountName.Value}'");
                template.Account = account;
            }

            if (request.Amount.IsSpecified)
            {
                template.Amount = request.Amount.Value;
            }

            if (request.Day.IsSpecified)
            {
                template.Day = request.Day.Value;
            }

            if (request.Description.IsSpecified)
            {
                template.Description = request.Description.Value
                    ?? throw new BadRequestException($"{nameof(request.Description)} must not be null");
            }

            if (extender != null)
            {
                await extender(template);
            }

            await _uow.SaveChangesAsync(ct);
        }

        public Task SpliceAsync(ITemplateSpliceRequest request, CancellationToken ct, Func<T, Task>? extender = null)
        {
            return CreateAsync(request, ct, async template =>
            {
                if (extender != null)
                {
                    await extender(template);
                }
                await _deleter.DeleteAsync(request, ct);
            });
        }
    }
}
