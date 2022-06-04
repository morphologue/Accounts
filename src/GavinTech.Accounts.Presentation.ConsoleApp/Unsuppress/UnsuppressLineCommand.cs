using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace GavinTech.Accounts.Presentation.ConsoleApp.Unsuppress;

[Command("unsuppress")]
[Subcommand(typeof(UnsuppressTransactionLineCommand))]
internal class UnsuppressLineCommand : LineCommandBase
{
    public RootLineCommand Parent { get; set; } = null!;

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct)
        => BombOutAsync(app, "Please specify a subcommand.");
}