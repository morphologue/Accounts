using System.Threading;
using System.Threading.Tasks;
using GavinTech.Accounts.Presentation.ConsoleApp.Add;
using McMaster.Extensions.CommandLineUtils;

namespace GavinTech.Accounts.Presentation.ConsoleApp;

[Command("dotnet GavinTech.Accounts.Presentation.ConsoleApp.dll")]
[VersionOption("Accounts 1.0.0")]
[Subcommand(typeof(AddLineCommand))]
internal class RootLineCommand : LineCommandBase
{
    [Option("--db", Description = "Override database path.")]
    [FileExists]
    public string? DatabasePath { get; set; }

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct)
        => BombOutAsync(app, "Please specify a command.");
}
