using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Morphologue.Accounts.Application.Transactions;
using Morphologue.Accounts.Domain.Primitives;

namespace Morphologue.Accounts.Presentation.ConsoleApp.Unsuppress;

[Command("transaction", "tran")]
internal class UnsuppressTransactionLineCommand : LineCommandBase
{
    public UnsuppressLineCommand Parent { get; set; } = null!;

    [Argument(0, Description = "ID of the transaction")]
    [Required]
    public string Id { get; set; } = string.Empty;

    [Argument(1, Description = "Date to unsuppress YYYY-MM-DD")]
    [RegularExpression(Regexen.Day)]
    [Required]
    public string Date { get; set; } = string.Empty;

    protected override async Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct) =>
        await MediateAsync(new RehabilitateTransactionCommand
        {
            Id = Id,
            Day = new Day(Date)
        }, Parent.Parent, ct);
}
