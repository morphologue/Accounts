using GavinTech.Accounts.Application.Interfaces.Persistence;
using GavinTech.Accounts.CrossCutting.DependencyInjection;
using GavinTech.Accounts.Infrastructure.Interfaces;
using GavinTech.Accounts.Infrastructure.Persistence;
using GavinTech.Accounts.Infrastructure.Persistence.EntityIdentification;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace GavinTech.Accounts.Infrastructure
{
    public class Layer : ScanningLayerBase
    {
        public class Options
        {
            public bool IsMultiUser { get; set; }
        }

        private readonly Options _options;

        public Layer(Options options) => _options = options;

        public override void RegisterDependencies(ServiceCollection services)
        {
            base.RegisterDependencies(services);

            services.AddSingleton(_options);

            services.AddDbContext<AccountsDbContext>(dbContextOptions =>
                dbContextOptions.UseSqlite("Data Source=accounts.db", sqliteOptions =>
                    sqliteOptions.MigrationsAssembly("GavinTech.Accounts.Migrations.Sqlite")));

            services.AddScoped(typeof(IEntityIdentifier<>), typeof(DefaultEntityIdentifier<>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // As IUserIdAccessor will only be registered by multi-user presentation layers, it
            // can't be injected directly. Instead we inject this delegate which is only to be
            // invoked in multi-user mode.
            services.AddScoped<Func<IUserIdAccessor>>(provider =>
                () => provider.GetRequiredService<IUserIdAccessor>());
        }

        public override Task InitialiseAsync(IServiceProvider scopedProvider)
        {
            return scopedProvider.GetRequiredService<AccountsDbContext>().Database.MigrateAsync();
        }
    }
}
