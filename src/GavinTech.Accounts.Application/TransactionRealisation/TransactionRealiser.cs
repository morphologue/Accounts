using GavinTech.Accounts.Application.Interfaces.Persistence;
using GavinTech.Accounts.CrossCutting.DependencyInjection;
using GavinTech.Accounts.Domain.Entities;
using GavinTech.Accounts.Domain.Extensions;
using GavinTech.Accounts.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Application.TransactionRealisation
{
    public interface ITransactionRealiser
    {
        Task<IEnumerable<Transaction>> RealiseAsync(
            Day? startDay,
            Day endDay,
            string? accountName,
            CancellationToken ct);
    }

    [ScopedService]
    internal class TransactionRealiser : ITransactionRealiser
    {
        private class BumpedRecurringTransactionTemplate : RecurringTransactionTemplate
        {
            internal Day OriginalDay;
            internal int Iteration;
        }

        private readonly IRepository<Account> _accountRepo;
        private readonly IRepository<TransactionTemplate> _templateRepo;

        public TransactionRealiser(
            IRepository<Account> accountRepo,
            IRepository<TransactionTemplate> templateRepo)
        {
            _accountRepo = accountRepo;
            _templateRepo = templateRepo;
        }

        public async Task<IEnumerable<Transaction>> RealiseAsync(
            Day? startDay,
            Day endDay,
            string? accountName,
            CancellationToken ct)
        {
            var orderedTemplates = (await _templateRepo.GetAsync(ct))
                .OrderBy(t => t.Amount)
                .ThenBy(t => t.Account.Name)
                .ToList();
            var allAccounts = await _accountRepo.GetAsync(ct);
            var flattenedAccountNamesWithClosure = allAccounts
                .Where(a => accountName == null || a.IsUnderName(accountName))
                .ToDictionary(a => a.Name, a => a.GetHierarchicalClosedAfter());

            if (orderedTemplates.Count == 0 || flattenedAccountNamesWithClosure.Count == 0)
            {
                return Enumerable.Empty<Transaction>();
            }

            // The following methods are lazy as realised transactions may be a never-ending
            // sequence.
            var trans = RealiseAll(orderedTemplates);
            return Filter(trans, startDay, endDay, flattenedAccountNamesWithClosure);
        }

        private static IEnumerable<Transaction> RealiseAll(IReadOnlyCollection<TransactionTemplate> templates)
        {
            var minDay = templates.Min(t => t.Day);
            var maxDay = templates.Max(t => t.Day);
            var futureRecurrences = new Dictionary<Day, ICollection<BumpedRecurringTransactionTemplate>>();
            for (var day = minDay; day <= maxDay || futureRecurrences.Count > 0; ++day)
            {
                foreach (var tran in ProcessDay(day, templates, futureRecurrences))
                {
                    yield return tran;
                }
            }
        }

        private static IEnumerable<Transaction> ProcessDay(
            Day day,
            IReadOnlyCollection<TransactionTemplate> templates,
            Dictionary<Day, ICollection<BumpedRecurringTransactionTemplate>> futureRecurrences)
        {
            var heutige = templates.Where(t => t.Day == day);
            if (futureRecurrences.TryGetValue(day, out var triggered))
            {
                heutige = triggered.Concat(heutige);
                futureRecurrences.Remove(day);
            }

            foreach (var tran in heutige)
            {
                if (tran is RecurringTransactionTemplate recurringTran)
                {
                    AdvanceRecurrence(recurringTran, futureRecurrences);
                    if (recurringTran.Tombstones.Contains(day))
                    {
                        continue;
                    }
                }

                yield return new()
                {
                    Day = day,
                    Amount = tran.Amount,
                    Description = tran.Description,
                    AccountName = tran.Account.Name
                };
            }
        }

        private static void AdvanceRecurrence(
            RecurringTransactionTemplate triggered,
            Dictionary<Day, ICollection<BumpedRecurringTransactionTemplate>> futureRecurrences)
        {
            var bumpedTriggered = triggered as BumpedRecurringTransactionTemplate;
            var originalDay = bumpedTriggered?.OriginalDay ?? triggered.Day;
            var originalDateTime = originalDay.ToDateTime();
            var originalMorgen = originalDateTime.AddDays(1);
            var iteration = (bumpedTriggered?.Iteration ?? 0) + 1;

            var nextDay = new Day(triggered.Basis switch
            {
                RecurrenceBasis.Daily => triggered.Day.ToDateTime().AddDays(triggered.Multiplicand),
                RecurrenceBasis.Monthly when originalDateTime.Month == originalMorgen.Month =>
                    originalDateTime.AddMonths((int)triggered.Multiplicand * iteration),
                RecurrenceBasis.Monthly =>
                    originalMorgen.AddMonths((int)triggered.Multiplicand * iteration).AddDays(-1),
                _ => throw new NotSupportedException(triggered.Basis.ToString())
            });

            if (triggered.UntilExcl.HasValue && nextDay >= triggered.UntilExcl)
            {
                return;
            }

            var nextRecurrence = new BumpedRecurringTransactionTemplate
            {
                Day = nextDay,
                Amount = triggered.Amount,
                Description = triggered.Description,
                Account = triggered.Account,
                Basis = triggered.Basis,
                Multiplicand = triggered.Multiplicand,
                UntilExcl = triggered.UntilExcl,
                Tombstones = triggered.Tombstones,
                OriginalDay = originalDay,
                Iteration = iteration
            };

            if (!futureRecurrences.TryGetValue(nextDay, out var recurrences))
            {
                futureRecurrences[nextDay] = recurrences = new List<BumpedRecurringTransactionTemplate>();
            }

            recurrences.Add(nextRecurrence);
        }

        private static IEnumerable<Transaction> Filter(
            IEnumerable<Transaction> trans,
            Day? startDay,
            Day endDay,
            Dictionary<string, Day?> flattenedAccountNamesWithClosure)
        {
            var runningTotal = new Amount();
            foreach (var tran in trans)
            {
                if (!flattenedAccountNamesWithClosure.TryGetValue(tran.AccountName, out var closedAfter)
                    || (startDay.HasValue && startDay.Value > tran.Day))
                {
                    continue;
                }

                if (endDay <= tran.Day
                    || (closedAfter.HasValue && closedAfter.Value < tran.Day))
                {
                    yield break;
                }

                tran.RunningTotal = runningTotal += tran.Amount;

                yield return tran;
            }
        }
    }
}
