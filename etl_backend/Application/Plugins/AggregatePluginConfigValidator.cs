using Application.Plugins.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects.PluginConfig;
using AggregateConfig = Domain.ValueObjects.PluginConfig.AggregateConfig;

namespace Application.Plugins;

public class AggregatePluginConfigValidator : IPluginConfigValidator
{
    public bool CanValidate(PluginType pluginType) => pluginType == PluginType.Aggregate;

    public PluginConfig ValidateAndParse(Dictionary<string, object> rawConfig)
    {
        var groupByColumns = rawConfig.TryGetValue("GroupByColumns", out var groupByObj)
            ? (groupByObj as IEnumerable<string>)?.ToList() ?? new List<string>()
            : new List<string>();

        var measures = rawConfig.TryGetValue("Measures", out var measuresObj)
            ? (measuresObj as IEnumerable<AggregateSpec>)?.Select(spec => new AggregateColumn(
                spec.Column,
                spec.Func.ToString()
            )).ToList() ?? new List<AggregateColumn>()
            : new List<AggregateColumn>();

        return new AggregateConfig(groupByColumns, measures);
    }
}