using GavinTech.Accounts.CrossCutting.DependencyInjection;
using GavinTech.Accounts.Presentation.ConsoleApp;
using Microsoft.Extensions.DependencyInjection;

var provider = await new ILayer[]
{
    new GavinTech.Accounts.Application.Layer(),
    new GavinTech.Accounts.Infrastructure.Layer(new()),
    new GavinTech.Accounts.Presentation.ConsoleApp.Layer()
}.BootstrapAsync();

await provider.GetRequiredService<IRepl>().ExecuteAsync();
