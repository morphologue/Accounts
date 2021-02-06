using System;
using System.Collections.Generic;
using System.Linq;
using GavinTech.Accounts.Domain.Entities;
using GavinTech.Accounts.Domain.Primitives;
using GavinTech.Accounts.Infrastructure.Persistence.ValueConversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GavinTech.Accounts.Infrastructure.Persistence.EntityConfigurations
{
    public class RecurringTransactionTemplateConfiguration : IEntityTypeConfiguration<RecurringTransactionTemplate>
    {
        public void Configure(EntityTypeBuilder<RecurringTransactionTemplate> builder)
        {
            builder.Property(e => e.UntilExcl)
                .HasConversion(DayConverter.Instance);
            builder.Property(e => e.Tombstones)
                .HasConversion(HashSetOfDayConverter.Instance, HashSetOfDayComparer.Instance);
        }
    }
}
