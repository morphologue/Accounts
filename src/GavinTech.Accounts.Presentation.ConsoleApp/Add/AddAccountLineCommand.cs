using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using GavinTech.Accounts.Application.Accounts;
using McMaster.Extensions.CommandLineUtils;

namespace GavinTech.Accounts.Presentation.ConsoleApp.Add;

[Command("account", "acct")]
internal class AddAccountLineCommand : LineCommandBase
{
    public AddLineCommand Parent { get; set; } = null!;

    [Option("-p|--parent", Description = "The parent account when creating a sub-account (default root)")]
    public string? ParentAccountName { get; set; }

    [Argument(0, Description = "Name of the account to add", Name = "Name")]
    [Required]
    public string AccountName { get; set; } = string.Empty;

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct)
        => MediateAsync(new CreateAccountCommand
        {
            Name = AccountName,
            Parent = ParentAccountName
        }, Parent.Parent, ct);
}
