using Domain.Enums;

namespace Domain.Entities;

public sealed class AggregateSpec
{
    public string Column { get; init; } = default!;
    public AggregateFunc Func { get; init; }
    public string? Alias { get; init; } 
}