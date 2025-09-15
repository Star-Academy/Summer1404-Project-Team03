using Application.Common.Exceptions;
using Application.Plugins.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects.PluginConfig;
using AggregateConfig = Domain.ValueObjects.PluginConfig.AggregateConfig;

public class PluginConfigParser : IPluginConfigParser
{
    public PluginConfig Parse(PluginType pluginType, object config)
    {
        return pluginType switch
        {
            PluginType.Filter when config is Dictionary<string, object> dict =>
                new FilterConfig(
                    dict.TryGetValue("Conditions", out var conditionsObj)
                        ? (conditionsObj as IEnumerable<FilterCondition>)?.ToArray()
                          ?? throw new UnprocessableEntityException("Invalid FilterConfig: Conditions is not IEnumerable<FilterCondition>")
                        : throw new UnprocessableEntityException("Invalid FilterConfig: Missing Conditions")
                ),

            PluginType.Aggregate when config is Dictionary<string, object> dict =>
                new AggregateConfig(
                    dict.TryGetValue("GroupByColumns", out var groupByObj)
                        ? (groupByObj as IReadOnlyList<string>)?.ToList() ?? new List<string>()
                        : new List<string>(),
                    dict.TryGetValue("Measures", out var measuresObj)
                        ? (measuresObj as IEnumerable<AggregateSpec>)?.Select(spec => new AggregateColumn(
                              spec.Column,
                              spec.Func.ToString() // ✅ Map AggregateFunc → string
                          )).ToList() // ✅ Convert to List<AggregateColumn>
                          ?? new List<AggregateColumn>()
                        : new List<AggregateColumn>()
                ),

            _ => throw new UnprocessableEntityException($"Invalid plugin type or config: {pluginType}")
        };
    }
}