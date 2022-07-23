using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using GavinTech.Accounts.Application.Transactions;
using GavinTech.Accounts.Domain.Primitives;
using McMaster.Extensions.CommandLineUtils;

namespace GavinTech.Accounts.Presentation.ConsoleApp.List;

[Command("transactions", "trans")]
internal class ListTransactionsLineCommand : LineCommandBase
{
    public ListLineCommand Parent { get; set; } = null!;

    [Option("--since", Description = "Since this day inclusive YYYY-MM-DD or 'ever' (default 1 month ago)")]
    [RegularExpression(Regexen.DayOrEver)]
    public string Since { get; set; } = DateTime.UtcNow.AddMonths(-1).ToString(Day.OneFormatToRuleThemAll);

    [Option("-y|--date", Description = "Up to this date exclusive YYYY-MM-DD (default tomorrow)")]
    [RegularExpression(Regexen.Day)]
    public string Date { get; set; } = DateTime.UtcNow.ToString(Day.OneFormatToRuleThemAll);

    [Option("-a|--account", Description = "Account (root by default)")]
    public string? AccountName { get; set; }

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct) =>
        MediateAsync(new GetTransactionsQuery
        {
            StartDayIncl = Since == "ever" ? null : new Day(Since),
            EndDayExcl = new Day(Date),
            AccountName = AccountName
        }, TabulateTransactions, Parent.Parent, ct);

    private static void TabulateTransactions(ICollection<Transaction> trans)
    {
        Console.WriteLine($"ID    Date       Amount      Balance     Description{string.Empty,29} Account");
        foreach (var tran in trans)
        {
            Console.WriteLine($"{tran.TemplateId,5} {tran.Day} ${tran.Amount,10} ${tran.RunningTotal,10} {tran.Description,-40} {tran.AccountId}");
        }
    }
}
