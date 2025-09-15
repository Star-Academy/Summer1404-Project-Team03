using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Application.Plugins.Abstractions;
using Domain.Entities;
using Domain.ValueObjects.PluginConfig;
using Infrastructure.DbConfig.Abstraction;
using AggregateConfig = Domain.Entities.AggregateConfig;

namespace Infrastructure.Plugins;

public class PluginWriter : IPluginWriter
{
    private readonly IEtlDbContextFactory _contextFactory;
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
                        options.DerivedTypes.Add(new JsonDerivedType(typeof(AggregateConfig), "AggregateConfig"));

                        typeInfo.PolymorphismOptions = options;
                    }
                }
            }
        }
    };

    public PluginWriter(IEtlDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    // public async Task AddAsync(Plugin plugin, CancellationToken ct)
    // {
    //     await using var ctx = _contextFactory.CreateWorkflowDbContext();
    //     ctx.Plugins.Add(plugin);
    //     await ctx.SaveChangesAsync(ct);
    // }
    public async Task AddAsync(Plugin plugin, CancellationToken ct)
    {
        // 🔥 CRITICAL: Log what JsonSerializer ACTUALLY produces
        var runtimeType = plugin.Config.GetType();
        var serializedJson = JsonSerializer.Serialize(plugin.Config, runtimeType, JsonOptions);

        Console.WriteLine($"RuntimeObject Type: {runtimeType}");
        Console.WriteLine($"Serialized JSON: {serializedJson}");
        Console.WriteLine($"JSON Length: {serializedJson.Length}");

        if (string.IsNullOrWhiteSpace(serializedJson) || serializedJson == "{}")
        {
            throw new InvalidOperationException("SERIALIZATION PRODUCED EMPTY JSON — ABORTING SAVE.");
        }

        await using var ctx = _contextFactory.CreateWorkflowDbContext();
        ctx.Plugins.Add(plugin);
        await ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Plugin plugin, CancellationToken ct)
    {
        await using var ctx = _contextFactory.CreateWorkflowDbContext();
        ctx.Plugins.Update(plugin);
        await ctx.SaveChangesAsync(ct);
    }
}