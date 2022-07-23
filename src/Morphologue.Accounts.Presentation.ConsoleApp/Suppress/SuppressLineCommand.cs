using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Morphologue.Accounts.Presentation.ConsoleApp.Suppress;

[Command("suppress")]
[Subcommand(typeof(SuppressTransactionLineCommand))]
internal class SuppressLineCommand : LineCommandBase
{
    public RootLineCommand Parent { get; set; } = null!;

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct)
        => BombOutAsync(app, "Please specify a subcommand.");
}
