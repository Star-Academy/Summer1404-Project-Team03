namespace WebApi.Plugins;

public class FilterSchemaResponse
{
    public List<string> FilterOps { get; set; } = new();
    public List<string> ValueTypeHints { get; set; } = new();
    public Dictionary<string, ConditionRule> ConditionRules { get; set; } = new();
    public FilterConfigExample Example { get; set; } = new();
}

public class ConditionRule
{
    public string[] Operations { get; set; } = Array.Empty<string>();
    public string[] RequiredFields { get; set; } = Array.Empty<string>();
    public string[] OptionalFields { get; set; } = Array.Empty<string>();
}

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