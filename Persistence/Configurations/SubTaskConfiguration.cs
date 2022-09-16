using Core.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Persistence.Configurations
{
    public class SubTaskConfiguration : IEntityTypeConfiguration<SubTask>
    {
        public void Configure(EntityTypeBuilder<SubTask> builder)
        {
            builder.ToTable(nameof(SubTask));
            builder.HasKey(t => t.Id);
            builder.Property<DateTime>("CreatedOn");
            builder.Property<DateTime>("LastUpdatedOn");
            builder.HasMany(s => s.Comments)
                .WithOne(c => c.SubTask);
                
        }
    }
}
