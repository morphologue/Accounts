using System;
using System.Threading;
using System.Threading.Tasks;
using GavinTech.Accounts.Application.Accounts;
using McMaster.Extensions.CommandLineUtils;

namespace GavinTech.Accounts.Presentation.ConsoleApp.Add;

[Command("account", "acct")]
internal class AddAccountLineCommand : LineCommandBase
{
    [Argument(0, Description = "Name of the account to add")]
    public string AccountName { get; set; } = string.Empty;

    [Option("-p|--parent", Description = "The parent account when creating a sub-account")]
    public string? ParentAccountName { get; set; }

    public AddLineCommand Parent { get; set; } = null!;

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct)
        => MediateAsync(new CreateAccountCommand
        {
            Name = AccountName,
            Parent = ParentAccountName
        }, Parent.Parent, ct);
}
