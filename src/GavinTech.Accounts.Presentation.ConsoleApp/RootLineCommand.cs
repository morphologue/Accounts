using System.Threading;
using System.Threading.Tasks;
using GavinTech.Accounts.Presentation.ConsoleApp.Add;
using GavinTech.Accounts.Presentation.ConsoleApp.Change;
using GavinTech.Accounts.Presentation.ConsoleApp.Delete;
using GavinTech.Accounts.Presentation.ConsoleApp.List;
using GavinTech.Accounts.Presentation.ConsoleApp.Replace;
using GavinTech.Accounts.Presentation.ConsoleApp.Suppress;
using GavinTech.Accounts.Presentation.ConsoleApp.Unsuppress;
using McMaster.Extensions.CommandLineUtils;

namespace GavinTech.Accounts.Presentation.ConsoleApp;

[Command("accounts")]
[VersionOption("Accounts 1.0.0")]
[Subcommand(typeof(AddLineCommand),
    typeof(ChangeLineCommand),
    typeof(DeleteLineCommand),
    typeof(ListLineCommand),
    typeof(ReplaceLineCommand),
    typeof(SuppressLineCommand),
    typeof(UnsuppressLineCommand))]
internal class RootLineCommand : LineCommandBase
{
    [Option("--db", Description = "Override database path.")]
    [FileExists]
    public string? DatabasePath { get; set; }

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct)
        => BombOutAsync(app, "Please specify a command.");
}
