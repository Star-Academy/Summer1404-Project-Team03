using System.Text.Json.Serialization;

namespace Domain.ValueObjects.PluginConfig;

public sealed class AggregateConfig : PluginConfig
{
    public List<string> GroupBy { get; set; } = new List<string>();
    public List<AggregateColumn> Aggregates { get; set; } = new  List<AggregateColumn>();

    public AggregateConfig(List<string> GroupBy,
        List<AggregateColumn> Aggregates)
    {
        this.GroupBy = GroupBy;
        this.Aggregates = Aggregates;
    }
}


public record AggregateColumn(
    string Column,     
    string Operation   
);