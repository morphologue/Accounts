using System;
using System.Threading;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Morphologue.Accounts.CrossCutting.DependencyInjection;

namespace Morphologue.Accounts.Presentation.ConsoleApp;

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
    protected static Task<int> MediateAsync(IRequest request, RootLineCommand root, CancellationToken ct) =>
        MediateAsync<IRequest, object>(request, null, root, ct);

    /// <summary>Fulfill a command by sending a request into MediatR and optionally handling the response.</summary>
    protected static Task<int> MediateAsync<TResponse>(
        IRequest<TResponse> request,
        Action<TResponse>? callback,
        RootLineCommand root,
        CancellationToken ct) => MediateAsync<IRequest<TResponse>, TResponse>(request, callback, root, ct);

    protected static async Task<int> MediateAsync<TRequest, TResponse>(
        TRequest request,
        Action<TResponse>? callback,
        RootLineCommand root,
        CancellationToken ct)
        where TRequest : IBaseRequest
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
            var response = await mediatr.Send(request, ct);
            callback?.Invoke((TResponse?)response ?? throw new InvalidOperationException("Queries may not return null"));
        }
        catch (Exception ex)
        {
            await Console.Error.WriteLineAsync(ex.Message);
            return 2;
        }

        return 0;
    }
}
