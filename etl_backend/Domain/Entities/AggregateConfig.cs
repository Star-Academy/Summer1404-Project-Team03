namespace Domain.Entities;

public sealed class AggregateConfig
{
    public List<string> GroupByColumns { get; init; } = new();
    public List<AggregateSpec> Measures { get; init; } = new();

    public AggregateConfig(List<string> groupByColumns, List<AggregateSpec> measures)
    {
        GroupByColumns = groupByColumns;
        Measures = measures;
    }
}