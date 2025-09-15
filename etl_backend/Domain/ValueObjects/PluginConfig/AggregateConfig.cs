namespace Domain.ValueObjects.PluginConfig;

public record AggregateConfig(
    List<string> GroupBy,           
    List<AggregateColumn> Aggregates 
) : PluginConfig;

public record AggregateColumn(
    string Column,     
    string Operation   
);