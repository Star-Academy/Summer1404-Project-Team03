namespace Application.Common.Configurations;

public class ColumnTypeConfiguration
{
    public List<string> CanonicalTypes { get; set; } = new();
    public Dictionary<string, string> TypeAliases { get; set; } = new();
}