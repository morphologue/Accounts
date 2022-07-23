using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace Morphologue.Accounts.Infrastructure.Persistence;

public class AccountsDbContextFactory : IDesignTimeDbContextFactory<AccountsDbContext>
{
    public AccountsDbContext CreateDbContext(string[] args)
    {
        var services = new ServiceCollection();
        new Layer(new()).RegisterDependencies(services);
        var provider = services.BuildServiceProvider();
        // This IServiceScope is intentionally never disposed. The context
        // returned by this method is used (only) by EF Core migrations, and
        // we don't know when they will be finished with it.
        var scope = provider.GetRequiredService<IServiceScopeFactory>().CreateScope();
        return scope.ServiceProvider.GetRequiredService<AccountsDbContext>();
    }
}
