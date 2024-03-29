﻿using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Morphologue.Accounts.Domain.Entities;

namespace Morphologue.Accounts.Infrastructure.Persistence;

public class AccountsDbContext : DbContext
{
    private readonly Layer.Options _layerOptions;

    public AccountsDbContext(
        DbContextOptions<AccountsDbContext> contextOptions,
        Layer.Options layerOptions) : base(contextOptions)
    {
        _layerOptions = layerOptions;
    }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<TransactionTemplate> TransactionTemplates => Set<TransactionTemplate>();

    public DbSet<RecurringTransactionTemplate> RecurringTransactionTemplates =>
        Set<RecurringTransactionTemplate>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // The following is like builder.ApplyConfigurationsFromAssembly() except passing
        // _layerOptions to the constructor of each configuration class.
        var openInterface = typeof(IEntityTypeConfiguration<>);
        var configTypes = typeof(AccountsDbContext).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Select(t => (
                ClassType: t,
                InterfaceType: t.GetInterfaces().FirstOrDefault(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == openInterface)
            ));
        var openMethod = builder.GetType().GetMethod(nameof(builder.ApplyConfiguration))
            ?? throw new ApplicationException($"The {nameof(builder.ApplyConfiguration)}() method is missing");
        foreach (var (classType, interfaceType) in configTypes)
        {
            if (interfaceType == null)
            {
                continue;
            }
            var closedMethod = openMethod.MakeGenericMethod(interfaceType.GenericTypeArguments[0]);
            var configInstance = Activator.CreateInstance(classType, _layerOptions);
            closedMethod.Invoke(builder, new[] {configInstance});
        }
    }
}
