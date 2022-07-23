using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GavinTech.Accounts.Application.Accounts;
using GavinTech.Accounts.Domain.Entities;
using McMaster.Extensions.CommandLineUtils;

namespace GavinTech.Accounts.Presentation.ConsoleApp.List;

[Command("accounts", "accts")]
internal class ListAccountsLineCommand : LineCommandBase
{
    public ListLineCommand Parent { get; set; } = null!;

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct) =>
        MediateAsync(new GetAccountsQuery(), TabulateAccounts, Parent.Parent, ct);

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
