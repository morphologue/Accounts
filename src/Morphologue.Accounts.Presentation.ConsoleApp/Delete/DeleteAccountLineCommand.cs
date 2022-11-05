using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Morphologue.Accounts.Application.Accounts;

namespace Morphologue.Accounts.Presentation.ConsoleApp.Delete;

[Command("account", "acct")]
internal class DeleteAccountLineCommand : LineCommandBase
{
    public DeleteLineCommand Parent { get; set; } = null!;

    [Argument(0, Description = "Name of the account to delete")]
    [Required]
    public string AccountName { get; set; } = string.Empty;

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct) =>
        MediateAsync(new DeleteAccountCommand {
            Name = AccountName
        }, Parent.Parent, ct);
}
