using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GavinTech.Accounts.Infrastructure.Persistence.EntityConfigurations
{
    public abstract class IdentifiedConfigurationBase<T> : IEntityTypeConfiguration<T>
        where T : class
    {
        protected IdentifiedConfigurationBase() { }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property<uint>("Id")
                .ValueGeneratedOnAdd();
            builder.HasKey("Id");
        }
    }
}
