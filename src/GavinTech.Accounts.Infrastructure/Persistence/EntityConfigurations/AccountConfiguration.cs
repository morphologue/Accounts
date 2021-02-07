using GavinTech.Accounts.Domain.Entities;
using GavinTech.Accounts.Infrastructure.Persistence.ValueConversion;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GavinTech.Accounts.Infrastructure.Persistence.EntityConfigurations
{
    public class AccountConfiguration : IdentifiedConfigurationBase<Account>
    {
        public override void Configure(EntityTypeBuilder<Account> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.ClosedAfter)
                .HasConversion(DayConverter.Instance);
        }
    }
}
