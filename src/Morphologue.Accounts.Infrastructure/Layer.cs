using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Morphologue.Accounts.Application.Interfaces.Persistence;
using Morphologue.Accounts.CrossCutting.DependencyInjection;
using Morphologue.Accounts.Infrastructure.Interfaces;
using Morphologue.Accounts.Infrastructure.Persistence;
using Morphologue.Accounts.Infrastructure.Persistence.EntityIdentification;

namespace Morphologue.Accounts.Infrastructure;

public class Layer : ScanningLayerBase
{
    public class Options
    {
        public bool IsMultiUser { get; set; }
        public string? DatabasePath { get; set; }
    }

    private readonly Options _options;

    public Layer(Options options) => _options = options;

    public override void RegisterDependencies(ServiceCollection services)
    {
        base.RegisterDependencies(services);

        services.AddSingleton(_options);

        services.AddDbContext<AccountsDbContext>(dbContextOptions =>
            dbContextOptions.UseSqlite($"Data Source={ResolveDatabasePath()}", sqliteOptions =>
                sqliteOptions.MigrationsAssembly("Morphologue.Accounts.Migrations.Sqlite")));

        services.AddScoped(typeof(IEntityIdentifier<>), typeof(DefaultEntityIdentifier<>));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // As IUserIdAccessor will only be registered by multi-user presentation layers, it
        // can't be injected directly. Instead we inject this delegate which is only to be
        // invoked in multi-user mode.
        services.AddScoped<Func<IUserIdAccessor>>(provider =>
            () => provider.GetRequiredService<IUserIdAccessor>());
    }

    public override async Task InitialiseAsync(IServiceProvider scopedProvider, CancellationToken ct)
    {
        var dbContext = scopedProvider.GetRequiredService<AccountsDbContext>();
        await dbContext.Database.MigrateAsync(ct);

        if (!await dbContext.Accounts.AnyAsync(ct))
        {
            // Create the root account.
            dbContext.Accounts.Add(new());
            await dbContext.SaveChangesAsync(ct);
        }
    }

    private string ResolveDatabasePath()
    {
        if (_options.DatabasePath == null)
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "morphologue", "accounts");
            Directory.CreateDirectory(dir);
            return Path.Combine(dir, "accounts.db");
        }

        if (_options.DatabasePath.Contains(';'))
        {
            throw new FileNotFoundException("The database file name contains an illegal character");
        }

        if (!File.Exists(_options.DatabasePath))
        {
            throw new FileNotFoundException("The specified database does not exist");
        }

        return _options.DatabasePath;
    }
}
