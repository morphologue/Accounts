using GavinTech.Accounts.Domain.Entities;
using GavinTech.Accounts.Infrastructure.Persistence.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GavinTech.Accounts.Infrastructure.Persistence.EntityConfigurations
{
    public class TransactionTemplateConfiguration : IEntityTypeConfiguration<TransactionTemplate>
    {
        public void Configure(EntityTypeBuilder<TransactionTemplate> builder)
        {
            builder.HasDiscriminator<bool>("IsRecurring")
                .HasValue<TransactionTemplate>(false)
                .HasValue<RecurringTransactionTemplate>(true);
            builder.Property(e => e.Day)
                .HasConversion(DayConverter.Instance);
            builder.Property(e => e.Amount)
                .HasConversion(AmountConverter.Instance);
        }
    }
}
