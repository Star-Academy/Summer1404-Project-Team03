namespace Domain.Entities;

public sealed class AggregateConfig
{
    public IReadOnlyList<string> GroupByColumns { get; init; } = Array.Empty<string>();
    public IReadOnlyList<AggregateSpec> Measures { get; init; } = Array.Empty<AggregateSpec>();
}