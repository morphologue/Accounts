using System;
using System.Threading.Tasks;
using GavinTech.Accounts.Application.DependencyInjection;

namespace GavinTech.Accounts.Presentation.ConsoleApp
{
    /// <summary>Read-eval-print loop</summary>
    public interface IRepl
    {
        Task ExecuteAsync();
    }

    [SingletonService]
    public class Repl : IRepl
    {
        public Task ExecuteAsync()
        {
            Console.WriteLine("Hello world");
            return Task.CompletedTask;
        }
    }
}
