using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Morphologue.Accounts.Application.Transactions;
using Morphologue.Accounts.Domain.Primitives;

namespace Morphologue.Accounts.Presentation.ConsoleApp.List;

[Command("transactions", "trans")]
internal class ListTransactionsLineCommand : LineCommandBase
{
    public ListLineCommand Parent { get; set; } = null!;

    [Option("--since", Description = "Since this day inclusive YYYY-MM-DD or 'ever' (default one month before date)")]
    [RegularExpression(Regexen.DayOrEver)]
    public string? Since { get; set; }

    [Option("-y|--date", Description = "Up to this date exclusive YYYY-MM-DD (default tomorrow)")]
    [RegularExpression(Regexen.Day)]
    public string Date { get; set; } = DateTime.Now.AddDays(1).ToString(Day.OneFormatToRuleThemAll);

    [Option("-t|--tail", Description = "Show last n rows only, first n for n < 0")]
    public int? Tail { get; set; }

    [Option("-a|--account", Description = "Account (root by default)")]
    public string? AccountName { get; set; }

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct)
    {
        var endDayExcl = new Day(Date);
        Day? sinceDayIncl = Since switch {
            "ever" => null,
            null => new Day(endDayExcl.ToDateTime().AddMonths(-1)),
            _ => new Day(Since)
        };

        return MediateAsync(new GetTransactionsQuery {
            StartDayIncl = sinceDayIncl,
            EndDayExcl = endDayExcl,
            AccountName = AccountName
        }, TabulateTransactions, Parent.Parent, ct);
    }

    private void TabulateTransactions(ICollection<Transaction> trans)
    {
        var tailedTrans = Tail switch {
            null => trans,
            < 0 => trans.Take(-Tail.Value),
            >= 0 => trans.TakeLast(Tail.Value)
        };

        Console.WriteLine($"ID     Date       Amount      Balance     Description{string.Empty,29} Account");
        foreach (var tran in tailedTrans)
        {
            Console.WriteLine($"{tran.TemplateId,5}{(tran.IsRecurring ? '*' : ' ')} {tran.Day} ${tran.Amount,10} ${tran.RunningTotal,10} {tran.Description,-40} {tran.AccountId}");
        }
    }
}
