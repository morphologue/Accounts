using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Morphologue.Accounts.Application.Templates;
using Morphologue.Accounts.Domain.Primitives;

namespace Morphologue.Accounts.Presentation.ConsoleApp.Replace;

[Command("transaction", "tran")]
internal class ReplaceTransactionLineCommand : LineCommandBase
{
    public ReplaceLineCommand Parent { get; set; } = null!;

    [Argument(0, Description = "ID of the transaction")]
    [Required]
    public string Id { get; set; } = string.Empty;

    [Argument(1, Description = "Date to replace YYYY-MM-DD")]
    [RegularExpression(Regexen.Day)]
    [Required]
    public string Date { get; set; } = string.Empty;

    [Argument(2, Description = "Amount")]
    [Required]
    [RegularExpression(Regexen.Amount)]
    public string Amount { get; set; } = string.Empty;

    [Option("--final", Description = "Whether to suppress subsequent dates (non-recurring replacement)")]
    public bool? EndRecurrence { get; set; }

    [Option("-a|--account", Description = "Account (root by default)")]
    public string? AccountName { get; set; }

    [Option("-d|--desc|--description", Description = "Description (blank by default)")]
    public string Description { get; set; } = string.Empty;

    [Option("-r|--recur|--recurring", Description = "Repeat, e.g. '1m' (monthly) or '14d' (fortnightly)")]
    [RegularExpression(Regexen.Recurrence)]
    public string? Recurrence { get; set; }

    [Option("--until", Description = "End recurrence before YYYY-MM-DD")]
    [RegularExpression(Regexen.Day)]
    public string? Until { get; set; }

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct)
    {
        if (Recurrence == null)
        {
            return Until != null
                ? BombOutAsync(app, "--until may only be used with --recurring")
                : ExecuteOneOff(ct);
        }

        return EndRecurrence.HasValue
            ? BombOutAsync(app, "--final is tautological with --recurring")
            : ExecuteRecurring(ct);
    }

    private async Task<int> ExecuteOneOff(CancellationToken ct) =>
        await MediateAsync(
            new SpliceTemplateCommand {
                Id = Id,
                AccountName = AccountName,
                Day = new Day(Date),
                EndRecurrence = EndRecurrence ?? false,
                Amount = Parsers.ParseAmount(Amount),
                Description = Description
            }, Parent.Parent, ct);

    private async Task<int> ExecuteRecurring(CancellationToken ct)
    {
        var (basis, multiplicand) = Parsers.ParseRecurrence(Recurrence ?? throw new InvalidOperationException());
        return await MediateAsync(
            new SpliceRecurringTemplateCommand {
                Id = Id,
                AccountName = AccountName,
                Day = new Day(Date),
                Amount = Parsers.ParseAmount(Amount),
                Description = Description,
                Basis = basis,
                Multiplicand = multiplicand,
                UntilExcl = Until == null ? null : new Day(Until)
            },
            Parent.Parent,
            ct);
    }
}
