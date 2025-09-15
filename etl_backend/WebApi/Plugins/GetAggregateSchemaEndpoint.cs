using Domain.Enums;
using FastEndpoints;

namespace WebApi.Plugins;

public class GetAggregateSchemaEndpoint : EndpointWithoutRequest<AggregateSchemaResponse>
{
    public override void Configure()
    {
        Get("api/plugins/aggregate/schema");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get aggregate plugin schema";
            s.Description = "Returns metadata for building aggregation UIs.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Response = new AggregateSchemaResponse
        {
            AggregateFunctions = Enum.GetNames<AggregateFunc>().ToList(),
            Example = new AggregateConfigExample
            {
                GroupByColumns = new[] { "department", "region" },
                Measures = new[]
                {
                    new AggregateSpecExample
                    {
                        Column = "salary",
                        Func = "Sum",
                        Alias = "total_salary"
                    },
                    new AggregateSpecExample
                    {
                        Column = "age",
                        Func = "Avg",
                        Alias = "average_age"
                    }
                }
            }
        };
    }
}

public class AggregateSchemaResponse
{
    public List<string> AggregateFunctions { get; set; } = new();
    public AggregateConfigExample Example { get; set; } = new();
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