using Domain.ValueObjects.PluginConfig;

namespace Domain.Entities;

public sealed class FilterConfig : PluginConfig
{
    public IReadOnlyList<FilterCondition> Conditions { get; init; } = Array.Empty<FilterCondition>();
}