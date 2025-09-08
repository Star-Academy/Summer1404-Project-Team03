using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbConfig.Configurations;

public class DataTableSchemaConfig : IEntityTypeConfiguration<DataTableSchema>
{
    public void Configure(EntityTypeBuilder<DataTableSchema> builder)
    {

        builder.HasKey(t => t.Id);

        builder.Property(t => t.TableName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.OriginalFileName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.HasIndex(t => t.TableName)
            .IsUnique();
        
        builder.HasMany(t => t.Columns)
            .WithOne(c => c.DataTable)
            .HasForeignKey(c => c.DataTableSchemaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}