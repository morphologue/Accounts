using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Morphologue.Accounts.Application.Accounts;
using Morphologue.Accounts.Domain.Entities;
using Morphologue.Accounts.Domain.Primitives;

namespace Morphologue.Accounts.Presentation.ConsoleApp.List;

[Command("accounts", "accts")]
internal class ListAccountsLineCommand : LineCommandBase
{
    public ListLineCommand Parent { get; set; } = null!;

    [Option("-y|--date", Description = "Show accounts open at this date YYYY-MM-DD (default today)")]
    [RegularExpression(Regexen.Day)]
    public string Date { get; set; } = DateTime.Now.ToString(Day.OneFormatToRuleThemAll);

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct) =>
        MediateAsync(new GetAccountsQuery {
            AsAtDay = new Day(Date)
        }, TabulateAccounts, Parent.Parent, ct);

    private static void TabulateAccounts(ICollection<Account> accounts)
    {
        var root = accounts.Single(a => a.Parent == null);
        TabulateAccount(accounts, root, 0);
    }

    private static void TabulateAccount(ICollection<Account> accounts, Account account, int indentationLevel)
    {
        Console.Write(new string(' ', indentationLevel++ * 2));
        Console.Write(account.Name);
        if (account.ClosedAfter.HasValue)
        {
            Console.Write($" (closed after {account.ClosedAfter})");
        }
        Console.WriteLine();

        foreach (var child in accounts.Where(a => a.Parent?.Name == account.Name).OrderBy(a => a.Name))
        {
            TabulateAccount(accounts, child, indentationLevel);
        }
    }
}
