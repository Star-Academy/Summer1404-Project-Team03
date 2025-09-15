using Domain.Enums;
using Infrastructure.Dtos;

namespace Application.Plugins.Abstractions;

public class FilterPluginSchemaProvider : IPluginSchemaProvider
{
    public PluginType PluginType => PluginType.Filter;

    public object GetSchema()
    {
        return new
        {
            FilterOps = Enum.GetNames<FilterOp>(),
            ValueTypeHints = Enum.GetNames<ValueTypeHint>(),
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
                    }
                }
            }
        };
    }
}