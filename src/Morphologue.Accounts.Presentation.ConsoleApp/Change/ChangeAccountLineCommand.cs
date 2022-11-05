using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Morphologue.Accounts.Application.Accounts;
using Morphologue.Accounts.Domain.Primitives;
using Morphologue.Accounts.Domain.Utilities;

namespace Morphologue.Accounts.Presentation.ConsoleApp.Change;

[Command("account", "acct")]
internal class ChangeAccountLineCommand : LineCommandBase
{
    public ChangeLineCommand Parent { get; set; } = null!;

    [Argument(0, Description = "Name of the account to change")]
    [Required]
    public string AccountName { get; set; } = string.Empty;

    [Option("-p|--parent", Description = "The new parent account")]
    public string? ParentAccountName { get; set; }

    [Option("--name", Description = "The new name of the account")]
    public string? NewName { get; set; }

    [Option("--close", Description = "Close after YYYY-MM-DD ('never' to reopen)")]
    [RegularExpression(Regexen.DayOrNever)]
    public string? ClosedAfter { get; set; }

    protected override Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct)
        => MediateAsync(new UpdateAccountCommand {
            Name = AccountName,
            Parent = new PatchBox<string> {
                IsSpecified = ParentAccountName != null,
                Value = ParentAccountName ?? string.Empty
            },
            NewName = new PatchBox<string> {
                IsSpecified = NewName != null,
                Value = NewName ?? string.Empty
            },
            ClosedAfter = new PatchBox<Day?> {
                IsSpecified = ClosedAfter != null,
                Value = ClosedAfter is null or "never" ? null : new Day(ClosedAfter)
            }
        }, Parent.Parent, ct);
}
