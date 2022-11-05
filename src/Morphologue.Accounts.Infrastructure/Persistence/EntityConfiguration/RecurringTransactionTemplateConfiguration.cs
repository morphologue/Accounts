using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Morphologue.Accounts.Domain.Entities;
using Morphologue.Accounts.Infrastructure.Persistence.ValueConversion;

namespace Morphologue.Accounts.Infrastructure.Persistence.EntityConfiguration;

internal class RecurringTransactionTemplateConfiguration : IEntityTypeConfiguration<RecurringTransactionTemplate>
{
    public RecurringTransactionTemplateConfiguration(Layer.Options _) { }

    public void Configure(EntityTypeBuilder<RecurringTransactionTemplate> builder)
    {
        builder.Property(e => e.UntilExcl)
            .HasConversion(DayConverter.Instance);
        builder.Property(e => e.Tombstones)
            .HasConversion(HashSetOfDayConverter.Instance, HashSetOfDayComparer.Instance);
    }
}
