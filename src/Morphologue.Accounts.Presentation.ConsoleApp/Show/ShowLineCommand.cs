using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace Morphologue.Accounts.Presentation.ConsoleApp.Show;

[Command("show")]
[Subcommand(typeof(ShowTransactionLineCommand))]
internal class ShowLineCommand : LineCommandBase
{
    public RootLineCommand Parent { get; set; } = null!;

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct)
        => BombOutAsync(app, "Please specify a subcommand.");
}
