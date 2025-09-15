namespace Domain.ValueObjects.PluginConfig;

public class AggregateConfig(
    List<string> GroupBy,           
    List<AggregateColumn> Aggregates 
) : PluginConfig;

public record AggregateColumn(
    string Column,     
    string Operation   
);