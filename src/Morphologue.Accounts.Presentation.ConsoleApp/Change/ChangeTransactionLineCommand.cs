using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Morphologue.Accounts.Application.Templates;
using Morphologue.Accounts.Domain.Entities;
using Morphologue.Accounts.Domain.Primitives;
using Morphologue.Accounts.Domain.Utilities;

namespace Morphologue.Accounts.Presentation.ConsoleApp.Change;

[Command("transaction", "tran")]
internal class ChangeTransactionLineCommand : LineCommandBase
{
    public ChangeLineCommand Parent { get; set; } = null!;

    [Argument(0, Description = "ID of the transaction to change")]
    [Required]
    public string Id { get; set; } = string.Empty;

    [Option("-a|--account", Description = "New account name")]
    public string? AccountName { get; set; }

    [Option("-y|--date", Description = "Transaction date YYYY-MM-DD")]
    [RegularExpression(Regexen.Day)]
    public string? Date { get; set; }

    [Option("--amount", Description = "Amount")]
    [RegularExpression(Regexen.Amount)]
    public string? Amount { get; set; }

    [Option("-d|--desc|--description", Description = "Description")]
    public string? Description { get; set; }

    [Option("-r|--recur|--recurring", Description = "Repeat, e.g. '1m' (monthly) or '14d' (fortnightly)")]
    [RegularExpression(Regexen.Recurrence)]
    public string? Recurrence { get; set; }

    [Option("--until", Description = "End recurrence before YYYY-MM-DD ('never' to uncap)")]
    [RegularExpression(Regexen.DayOrNever)]
    public string? Until { get; set; }

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct) =>
        Recurrence == null && Until == null ? ExecuteOneOffAsync(ct) : ExecuteRecurringAsync(ct);

    private async Task<int> ExecuteOneOffAsync(CancellationToken ct) =>
        await MediateAsync(new UpdateTemplateCommand {
            Id = Id,
            AccountName = new PatchBox<string> {
                IsSpecified = AccountName != null,
                Value = AccountName ?? string.Empty
            },
            Amount = new PatchBox<Amount> {
                IsSpecified = Amount != null,
                Value = Amount == null ? new Amount() : Parsers.ParseAmount(Amount)
            },
            Day = new PatchBox<Day> {
                IsSpecified = Date != null,
                Value = Date == null ? new Day() : new Day(Date)
            },
            Description = new PatchBox<string> {
                IsSpecified = Description != null,
                Value = Description ?? string.Empty
            }
        }, Parent.Parent, ct);

    private async Task<int> ExecuteRecurringAsync(CancellationToken ct)
    {
        var parsed = Recurrence == null ? null : Parsers.ParseRecurrence(Recurrence);
        return await MediateAsync(new UpdateRecurringTemplateCommand {
            Id = Id,
            AccountName = new PatchBox<string> {
                IsSpecified = AccountName != null,
                Value = AccountName ?? string.Empty
            },
            Amount = new PatchBox<Amount> {
                IsSpecified = Amount != null,
                Value = Amount == null ? new Amount() : Parsers.ParseAmount(Amount)
            },
            Day = new PatchBox<Day> {
                IsSpecified = Date != null,
                Value = Date == null ? new Day() : new Day(Date)
            },
            Description = new PatchBox<string> {
                IsSpecified = Description != null,
                Value = Description ?? string.Empty
            },
            Basis = new PatchBox<RecurrenceBasis> {
                IsSpecified = Recurrence != null,
                Value = parsed?.Item1 ?? default
            },
            Multiplicand = new PatchBox<uint> {
                IsSpecified = Recurrence != null,
                Value = parsed?.Item2 ?? default
            },
            UntilExcl = new PatchBox<Day?> {
                IsSpecified = Until != null,
                Value = Until is null or "never" ? null : new Day(Until)
            }
        }, Parent.Parent, ct);
    }
}
