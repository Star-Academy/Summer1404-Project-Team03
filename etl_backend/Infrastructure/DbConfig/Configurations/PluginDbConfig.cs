using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Domain.Entities;
using Domain.ValueObjects.PluginConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AggregateConfig = Domain.Entities.AggregateConfig;

namespace Infrastructure.DbConfig.Configurations;

public class PluginDbConfig : IEntityTypeConfiguration<Plugin>
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = false,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        TypeInfoResolver = new DefaultJsonTypeInfoResolver()
        {
            Modifiers =
            {
                static typeInfo =>
                {
                    if (typeInfo.Type == typeof(PluginConfig))
                    {
                        var options = new JsonPolymorphismOptions
                        {
                            TypeDiscriminatorPropertyName = "$type",
                            IgnoreUnrecognizedTypeDiscriminators = false,
                            UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization
                        };

                        options.DerivedTypes.Add(new JsonDerivedType(typeof(FilterConfig), "FilterConfig"));
                        // options.DerivedTypes.Add(new JsonDerivedType(typeof(AggregateConfig), "AggregateConfig")); // ← ADD THIS

                        typeInfo.PolymorphismOptions = options;
                    }
                }
            }
        }
    };
    public void Configure(EntityTypeBuilder<Plugin> builder)
    {
        builder.ToTable("Plugins");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.Property(p => p.WorkflowId).IsRequired();
        builder.Property(p => p.PluginType) 
            .IsRequired()
            .HasConversion<string>(); 
        builder.Property(p => p.Config)
            .IsRequired()
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonOptions),  
                v => JsonSerializer.Deserialize<PluginConfig>(v, JsonOptions)!
            );
        builder.Property(p => p.Order).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();
        builder.Property(p => p.UpdatedAt).IsRequired();

        builder.HasIndex(p => p.WorkflowId);
        builder.HasIndex(p => new { p.WorkflowId, p.Order }).IsUnique();
    }
}