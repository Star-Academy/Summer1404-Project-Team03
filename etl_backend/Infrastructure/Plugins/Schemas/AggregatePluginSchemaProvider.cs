using Domain.Enums;

namespace Application.Plugins.Abstractions;

public class AggregatePluginSchemaProvider : IPluginSchemaProvider
{
    public PluginType PluginType => PluginType.Aggregate;

    public object GetSchema()
    {
        return new
        {
            AggregateFunctions = Enum.GetNames<AggregateFunc>(),
            Example = new AggregateConfigExample
            {
                GroupByColumns = new[] { "department" },
                Measures = new[]
                {
                    new AggregateSpecExample
                    {
                        Column = "salary",
                        Func = "Sum",
                        Alias = "total_salary"
                    }
                }
            }
        };
    }
}

public class AggregateConfigExample
{
    public string[] GroupByColumns { get; set; } = Array.Empty<string>();
    public AggregateSpecExample[] Measures { get; set; } = Array.Empty<AggregateSpecExample>();
}

public class AggregateSpecExample
{
    public string Column { get; set; } = string.Empty;
    public string Func { get; set; } = string.Empty;
    public string? Alias { get; set; }
}