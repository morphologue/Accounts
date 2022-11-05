using System;

namespace Morphologue.Accounts.CrossCutting.DependencyInjection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class SingletonServiceAttribute : Attribute
{ }
