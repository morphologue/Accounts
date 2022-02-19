using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using GavinTech.Accounts.Application.Templates;
using GavinTech.Accounts.Domain.Entities;
using GavinTech.Accounts.Domain.Primitives;
using McMaster.Extensions.CommandLineUtils;

namespace GavinTech.Accounts.Presentation.ConsoleApp.Add;

[Command("transaction", "tran")]
internal class AddTransactionLineCommand : LineCommandBase
{
    public AddLineCommand Parent { get; set; } = null!;

    [Option("-a|--account", Description = "Account (root by default)")]
    public string AccountName { get; set; } = Account.RootAccountName;

    [Option("-y|--date", Description = "Transaction date YYYY-MM-DD (today by default)")]
    [RegularExpression(Regexen.Day)]
    public string Date { get; set; } = DateTime.Now.ToString(Day.OneFormatToRuleThemAll);

    [Option("-d|--desc|--description", Description = "Description (blank by default)")]
    public string Description { get; set; } = string.Empty;

    [Option("-r|--recurring", Description = "Repeat, e.g. '1m' (monthly) or '14d' (fortnightly)")]
    [RegularExpression(Regexen.Recurrence)]
    public string? Recurrence { get; set; }

    [Option("--until", Description = "End recurrence before YYYY-MM-DD")]
    public string? Until { get; set; }

    [Argument(0, Description = "Amount")]
    [Required]
    [RegularExpression(Regexen.Amount)]
    public string Amount { get; set; } = string.Empty;

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct)
    {
        if (Recurrence == null)
        {
            return Until != null
                ? BombOutAsync(app, "--until may only be used with --recurring")
                : ExecuteOneOff(ct);
        }

        return ExecuteRecurring(ct);
    }

    private async Task<int> ExecuteOneOff(CancellationToken ct) =>
        await MediateAsync(
            new CreateTemplateCommand
            {
                AccountName = AccountName,
                Day = new Day(Date),
                Amount = Parsers.ParseAmount(Amount),
                Description = Description
            },
            Parent.Parent, ct);

    private async Task<int> ExecuteRecurring(CancellationToken ct)
    {
        var (basis, multiplicand) = Parsers.ParseRecurrence(Recurrence ?? throw new InvalidOperationException());
        return await MediateAsync(
            new CreateRecurringTemplateCommand
            {
                AccountName = AccountName,
                Day = new Day(Date),
                Amount = Parsers.ParseAmount(Amount),
                Description = Description,
                Basis = basis,
                Multiplicand = multiplicand,
                UntilExcl = Until == null ? null : new Day(Until)
            },
            Parent.Parent, ct);
    }
}
