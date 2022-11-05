using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Morphologue.Accounts.Domain.Entities;
using Morphologue.Accounts.Infrastructure.Persistence.ValueConversion;

namespace Morphologue.Accounts.Infrastructure.Persistence.EntityConfiguration;

internal class AccountConfiguration : IdentifiedConfigurationBase<Account>
{
    public AccountConfiguration(Layer.Options layerOptions) : base(layerOptions) { }

    public override void Configure(EntityTypeBuilder<Account> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.ClosedAfter)
            .HasConversion(DayConverter.Instance);
        builder.HasIndex(e => e.Name)
            .IsUnique();
    }
}
