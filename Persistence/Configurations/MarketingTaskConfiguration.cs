using Core.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations
{
    public class MarketingTaskConfiguration : IEntityTypeConfiguration<MarketingTask>
    {
        public void Configure(EntityTypeBuilder<MarketingTask> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property<DateTime>("CreatedOn");
            builder.Property<DateTime>("LastUpdatedOn");
            builder.HasMany( m => m.SubTasks)
                .WithOne(s => s.MarketingTask);
        }
    }
}
