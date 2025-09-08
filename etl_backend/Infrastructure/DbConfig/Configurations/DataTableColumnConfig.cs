using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbConfig.Configurations;

public class DataTableColumnConfig : IEntityTypeConfiguration<DataTableColumn>
{
    public void Configure(EntityTypeBuilder<DataTableColumn> builder)
    {

        builder.HasKey(c => c.Id);

        builder.Property(c => c.ColumnName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.ColumnType)
            .HasMaxLength(100)
            .HasDefaultValue("string");

        builder.Property(c => c.OrdinalPosition)
            .IsRequired();
        
        builder.HasIndex(c => new { c.DataTableSchemaId, c.ColumnName }).IsUnique();
    }
}