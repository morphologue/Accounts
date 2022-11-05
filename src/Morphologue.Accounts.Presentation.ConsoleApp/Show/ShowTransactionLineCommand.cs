using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Morphologue.Accounts.Application.Templates;
using Morphologue.Accounts.Domain.Entities;

namespace Morphologue.Accounts.Presentation.ConsoleApp.Show;

[Command("transaction", "tran")]
internal class ShowTransactionLineCommand : LineCommandBase
{
    private const int FirstColumnWidth = -13;

    public ShowLineCommand Parent { get; set; } = null!;

    [Argument(0, Description = "ID of the transaction to show")]
    [Required]
    public string Id { get; set; } = string.Empty;

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct) =>
        MediateAsync(new GetTemplateQuery {
            Id = Id
        }, PrettifyTemplate, Parent.Parent, ct);

    private static void PrettifyTemplate(TransactionTemplate template)
    {
        Console.WriteLine($"{"Start Date:",FirstColumnWidth}{template.Day}");
        Console.WriteLine($"{"Amount:",FirstColumnWidth}${template.Amount}");
        Console.WriteLine($"{"Description:",FirstColumnWidth}{template.Description}");
        Console.WriteLine($"{"Account:",FirstColumnWidth}{template.Account.Name}");

        if (template is not RecurringTransactionTemplate recurring)
        {
            Console.WriteLine("Does not recur");
            return;
        }

        Console.Write($"Recurs every {recurring.Multiplicand} ");
        Console.Write(recurring.Basis switch {
            RecurrenceBasis.Daily => "day",
            RecurrenceBasis.Monthly => "month",
            _ => throw new NotSupportedException($"Unknown recurrence basis: {recurring.Basis}")
        });
        if (recurring.Multiplicand != 1)
        {
            Console.Write("s");
        }

        if (recurring.UntilExcl.HasValue)
        {
            Console.Write($" until {recurring.UntilExcl} (excl.)");
        }

        switch (recurring.Tombstones.Count)
        {
            case 0:
                Console.WriteLine();
                break;
            case 1:
                Console.WriteLine($" except {recurring.Tombstones.Single()}");
                break;
            default:
                Console.Write(" except the following: ");
                Console.WriteLine(string.Join(", ", recurring.Tombstones.OrderBy(d => d.Offset)));
                break;
        }
    }
}
