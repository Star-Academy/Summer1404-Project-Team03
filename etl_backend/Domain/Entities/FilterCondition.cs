using Domain.Enums;

namespace Domain.Entities;

public sealed class FilterCondition
{
    public string Column { get; init; } = default!;
    public FilterOp Op { get; init; }
    public ValueTypeHint TypeHint { get; init; } = ValueTypeHint.String;

    public string? Value { get; init; }             // single value ops
    public string? Value2 { get; init; }            // upper for Between
    public IEnumerable<string>? Values { get; init; } // IN list
}