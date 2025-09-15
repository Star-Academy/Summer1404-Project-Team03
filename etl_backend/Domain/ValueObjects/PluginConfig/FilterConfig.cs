using Domain.ValueObjects.PluginConfig;

namespace Domain.Entities;

public sealed class FilterConfig : PluginConfig
{
    public FilterConfig(){}
    public IReadOnlyList<FilterCondition> Conditions { get; init; } = Array.Empty<FilterCondition>();
    public FilterConfig(IReadOnlyList<FilterCondition> conditions)
    {
        Conditions = conditions;
    }
}