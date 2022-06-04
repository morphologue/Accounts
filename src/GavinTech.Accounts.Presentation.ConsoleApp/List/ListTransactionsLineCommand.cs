using System;
using System.Collections.Generic;
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

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct) =>
        MediateAsync(new GetTransactionsQuery
        {
            EndDayExcl = new Day(DateTime.UtcNow.AddDays(1))
        }, TabulateTransactions, Parent.Parent, ct);

    private static void TabulateTransactions(ICollection<Transaction> trans)
    {
        Console.WriteLine($"ID    Date       Amount     Description{string.Empty,29} Account");
        foreach (var tran in trans)
        {
            Console.WriteLine($"{tran.TemplateId,5} {tran.Day} ${tran.Amount,9} {tran.Description,-40} {tran.AccountId}");
        }
    }
}
