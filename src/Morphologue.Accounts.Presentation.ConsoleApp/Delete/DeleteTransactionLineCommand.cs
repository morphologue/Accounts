using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Morphologue.Accounts.Application.Templates;

namespace Morphologue.Accounts.Presentation.ConsoleApp.Delete;

[Command("transaction", "tran")]
internal class DeleteTransactionLineCommand : LineCommandBase
{
    private DeleteLineCommand Parent { get; set; } = null!;

    [Argument(0, Description = "ID of the transaction to delete")]
    [Required]
    public string Id { get; set; } = string.Empty;

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct) =>
        MediateAsync(new DeleteTemplateCommand {
            Id = Id
        }, Parent.Parent, ct);
}
