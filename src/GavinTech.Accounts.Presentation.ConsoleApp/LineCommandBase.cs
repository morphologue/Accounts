using System;
using System.Threading;
using System.Threading.Tasks;
using GavinTech.Accounts.CrossCutting.DependencyInjection;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GavinTech.Accounts.Presentation.ConsoleApp;

[HelpOption]
internal abstract class LineCommandBase
{
    protected abstract Task<int> OnExecuteAsync(CommandLineApplication app, CancellationToken ct);

    protected static async Task<int> BombOutAsync(CommandLineApplication app, string message)
    {
        await Console.Error.WriteLineAsync(message);
        await Console.Error.WriteLineAsync();
        app.ShowHelp();
        return 1;
    }

    /// <summary>Fulfill a command by sending a request into MediatR.</summary>
    protected static async Task<int> MediateAsync(IRequest request, RootLineCommand root, CancellationToken ct)
    {
        var infraOptions = new Infrastructure.Layer.Options
        {
            DatabasePath = root.DatabasePath
        };

        var provider = await new ILayer[]
        {
            new Application.Layer(),
            new Infrastructure.Layer(infraOptions)
        }.BootstrapAsync(ct);

        await using var scope = provider.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
        var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();
        try
        {
            await mediatr.Send(request, ct);
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync(ex.Message);
            return 2;
        }

        return 0;
    }
}
