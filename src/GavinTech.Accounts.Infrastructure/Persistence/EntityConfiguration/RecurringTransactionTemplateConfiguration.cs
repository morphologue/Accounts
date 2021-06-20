using GavinTech.Accounts.Domain.Entities;
using GavinTech.Accounts.Infrastructure.Persistence.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GavinTech.Accounts.Infrastructure.Persistence.EntityConfiguration
{
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
}
