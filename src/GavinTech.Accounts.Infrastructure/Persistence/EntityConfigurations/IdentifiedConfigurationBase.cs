using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace GavinTech.Accounts.Infrastructure.Persistence.EntityConfigurations
{
    internal abstract class IdentifiedConfigurationBase<T> : IEntityTypeConfiguration<T>
        where T : class
    {
        private readonly Layer.Options _layerOptions;

        protected IdentifiedConfigurationBase(Layer.Options layerOptions) =>
            _layerOptions = layerOptions;

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property<uint>("Id")
                .ValueGeneratedOnAdd();
            builder.HasKey("Id");
            if (_layerOptions.IsMultiUser)
            {
                builder.Property<Guid>(Constants.UserIdColumnName);
            }
        }
    }
}
