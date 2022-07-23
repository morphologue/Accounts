using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Morphologue.Accounts.Presentation.ConsoleApp.Delete;

[Command("delete")]
[Subcommand(typeof(DeleteTransactionLineCommand), typeof(DeleteAccountLineCommand))]
internal class DeleteLineCommand : LineCommandBase
{
    public RootLineCommand Parent { get; set; } = null!;

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct)
        => BombOutAsync(app, "Please specify a subcommand.");
}
