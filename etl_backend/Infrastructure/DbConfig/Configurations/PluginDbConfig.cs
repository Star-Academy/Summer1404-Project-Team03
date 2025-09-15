using System.Text.Json;
using Domain.Entities;
using Domain.ValueObjects.PluginConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DbConfig.Configurations;

public class PluginDbConfig : IEntityTypeConfiguration<Plugin>
{
    public void Configure(EntityTypeBuilder<Plugin> builder)
    {
        builder.ToTable("Plugins");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.Property(p => p.WorkflowId).IsRequired();
        builder.Property(p => p.PluginType).IsRequired().HasMaxLength(64);
        builder.Property(p => p.Config)
            .IsRequired()
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                v => JsonSerializer.Deserialize<PluginConfig>(v, (JsonSerializerOptions)null!)!
            );
        builder.Property(p => p.Order).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt).IsRequired();

        builder.HasIndex(p => p.WorkflowId);
        builder.HasIndex(p => new { p.WorkflowId, p.Order }).IsUnique();
    }
}