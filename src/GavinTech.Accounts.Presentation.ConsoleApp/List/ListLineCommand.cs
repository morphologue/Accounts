using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace GavinTech.Accounts.Presentation.ConsoleApp.List;

[Command("list")]
[Subcommand(typeof(ListAccountsLineCommand), typeof(ListTransactionsLineCommand))]
internal class ListLineCommand : LineCommandBase
{
    public RootLineCommand Parent { get; set; } = null!;

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct)
        => BombOutAsync(app, "Please specify a subcommand.");
}
