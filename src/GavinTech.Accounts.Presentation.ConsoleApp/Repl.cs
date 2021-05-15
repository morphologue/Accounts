﻿using GavinTech.Accounts.CrossCutting.DependencyInjection;
using System;
using System.Threading.Tasks;

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
