using Domain.Enums;
using FastEndpoints;

namespace WebApi.Plugins;

public class GetFilterSchemaEndpoint : EndpointWithoutRequest<FilterSchemaResponse>
{
    public override void Configure()
    {
        Get("api/plugins/filter/schema");
        AllowAnonymous(); 
        Summary(s =>
        {
            s.Summary = "Get filter plugin schema";
            s.Description = "Returns metadata for building filter UIs.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Response = new FilterSchemaResponse
        {
            FilterOps = Enum.GetNames<FilterOp>().ToList(),
            ValueTypeHints = Enum.GetNames<ValueTypeHint>().ToList(),
            ConditionRules = new Dictionary<string, ConditionRule>
            {
                ["SingleValueOps"] = new ConditionRule
                {
                    Operations = new[] { "Eq", "Ne", "Gt", "Ge", "Lt", "Le", "Contains", "StartsWith", "EndsWith" },
                    RequiredFields = new[] { "Column", "Op", "TypeHint", "Value" },
                    OptionalFields = new[] { "Value2" }
                },
                ["Between"] = new ConditionRule
                {
                    Operations = new[] { "Between" },
                    RequiredFields = new[] { "Column", "Op", "TypeHint", "Value", "Value2" },
                    OptionalFields = Array.Empty<string>()
                },
                ["In"] = new ConditionRule
                {
                    Operations = new[] { "In" },
                    RequiredFields = new[] { "Column", "Op", "TypeHint", "Values" },
                    OptionalFields = Array.Empty<string>()
                },
                ["NullOps"] = new ConditionRule
                {
                    Operations = new[] { "IsNull", "IsNotNull" },
                    RequiredFields = new[] { "Column", "Op", "TypeHint" },
                    OptionalFields = new[] { "Value", "Value2", "Values" }
                }
            },
            Example = new FilterConfigExample
            {
                Conditions = new[]
                {
                    new FilterConditionExample
                    {
                        Column = "age",
                        Op = "Gt",
                        TypeHint = "Int",
                        Value = "18"
                    },
                    new FilterConditionExample
                    {
                        Column = "name",
                        Op = "Contains",
                        TypeHint = "String",
                        Value = "John"
                    },
                    new FilterConditionExample
                    {
                        Column = "status",
                        Op = "In",
                        TypeHint = "String",
                        Values = new[] { "active", "pending" }
                    },
                    new FilterConditionExample
                    {
                        Column = "created_at",
                        Op = "Between",
                        TypeHint = "Timestamp",
                        Value = "2024-01-01T00:00:00Z",
                        Value2 = "2024-12-31T23:59:59Z"
                    }
                }
            }
        };
    }
}