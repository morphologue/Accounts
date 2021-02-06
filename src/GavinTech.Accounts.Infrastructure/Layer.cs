using System;
using System.Threading.Tasks;
using GavinTech.Accounts.Application.DependencyInjection;
using GavinTech.Accounts.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GavinTech.Accounts.Infrastructure
{
    public class Layer : ScanningLayerBase
    {
        public override void RegisterDependencies(ServiceCollection services, string[] args)
        {
            base.RegisterDependencies(services, args);
            services.AddDbContext<AccountsDbContext>(dbContextOptions =>
                dbContextOptions.UseSqlite("Data Source=accounts.db", sqliteOptions =>
                    sqliteOptions.MigrationsAssembly("GavinTech.Accounts.Migrations.Sqlite")));
        }

        public override Task InitialiseAsync(IServiceProvider scopedProvider)
        {
            return scopedProvider.GetRequiredService<AccountsDbContext>().Database.MigrateAsync();
        }
    }
}
