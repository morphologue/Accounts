using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Morphologue.Accounts.Presentation.ConsoleApp.Add;
using Morphologue.Accounts.Presentation.ConsoleApp.Change;
using Morphologue.Accounts.Presentation.ConsoleApp.Delete;
using Morphologue.Accounts.Presentation.ConsoleApp.List;
using Morphologue.Accounts.Presentation.ConsoleApp.Replace;
using Morphologue.Accounts.Presentation.ConsoleApp.Show;
using Morphologue.Accounts.Presentation.ConsoleApp.Suppress;
using Morphologue.Accounts.Presentation.ConsoleApp.Unsuppress;

namespace Morphologue.Accounts.Presentation.ConsoleApp;

[Command("accounts")]
[VersionOption("Accounts 1.1.1")]
[Subcommand(typeof(AddLineCommand),
    typeof(ChangeLineCommand),
    typeof(DeleteLineCommand),
    typeof(ListLineCommand),
    typeof(ReplaceLineCommand),
    typeof(SuppressLineCommand),
    typeof(ShowLineCommand),
    typeof(UnsuppressLineCommand))]
internal class RootLineCommand : LineCommandBase
{
    [Option("--db", Description = "Override database path.")]
    [FileExists]
    public string? DatabasePath { get; set; }

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct)
        => BombOutAsync(app, "Please specify a command.");
}
