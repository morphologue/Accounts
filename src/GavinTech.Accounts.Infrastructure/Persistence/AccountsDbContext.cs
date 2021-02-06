using GavinTech.Accounts.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GavinTech.Accounts.Infrastructure.Persistence
{
    public class AccountsDbContext : DbContext
    {
        public AccountsDbContext(DbContextOptions<AccountsDbContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<TransactionTemplate> TransactionTemplates { get; set; }
        public DbSet<RecurringTransactionTemplate> RecurringTransactionTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) =>
            builder.ApplyConfigurationsFromAssembly(typeof(AccountsDbContext).Assembly);
    }
}
