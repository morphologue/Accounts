using System;
namespace GavinTech.Accounts.CrossCutting.DependencyInjection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class ScopedServiceAttribute : Attribute
{
}