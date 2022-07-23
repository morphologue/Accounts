using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Morphologue.Accounts.Application.Transactions;
using Morphologue.Accounts.Domain.Primitives;

namespace Morphologue.Accounts.Presentation.ConsoleApp.Suppress;

[Command("transaction", "tran")]
internal class SuppressTransactionLineCommand : LineCommandBase
{
    public SuppressLineCommand Parent { get; set; } = null!;

    [Argument(0, Description = "ID of the transaction")]
    [Required]
    public string Id { get; set; } = string.Empty;

    [Argument(1, Description = "Date to suppress YYYY-MM-DD")]
    [RegularExpression(Regexen.Day)]
    [Required]
    public string Date { get; set; } = string.Empty;

    [Option("--final", Description = "Whether to suppress subsequent dates")]
    public bool EndRecurrence { get; set; }

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct) =>
        await MediateAsync(new DeleteTransactionCommand
        {
            Id = Id,
            Day = new Day(Date),
            EndRecurrence = EndRecurrence
        }, Parent.Parent, ct);
}
