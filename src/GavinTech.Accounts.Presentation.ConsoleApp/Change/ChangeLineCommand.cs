using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace GavinTech.Accounts.Presentation.ConsoleApp.Change;

[Command("change")]
[Subcommand(typeof(ChangeAccountLineCommand), typeof(ChangeTransactionLineCommand))]
internal class ChangeLineCommand : LineCommandBase
{
    public RootLineCommand Parent { get; set; } = null!;

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct)
        => BombOutAsync(app, "Please specify a subcommand.");
}
