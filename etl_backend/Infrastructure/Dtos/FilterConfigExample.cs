namespace Infrastructure.Dtos;

public class FilterConfigExample
{
    public FilterConditionExample[] Conditions { get; set; } = Array.Empty<FilterConditionExample>();
}

public class FilterConditionExample
{
    public string Column { get; set; } = string.Empty;
    public string Op { get; set; } = string.Empty;
    public string TypeHint { get; set; } = string.Empty;
    public string? Value { get; set; }
    public string? Value2 { get; set; }
    public string[]? Values { get; set; }
}