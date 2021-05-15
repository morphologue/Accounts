using GavinTech.Accounts.Domain.Entities;
using GavinTech.Accounts.Infrastructure.Persistence.ValueConversion;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GavinTech.Accounts.Infrastructure.Persistence.EntityConfigurations
{
    internal class TransactionTemplateConfiguration : IdentifiedConfigurationBase<TransactionTemplate>
    {
        public TransactionTemplateConfiguration(Layer.Options layerOptions) : base(layerOptions) { }

        public override void Configure(EntityTypeBuilder<TransactionTemplate> builder)
        {
            base.Configure(builder);
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
