namespace WebApi.Files.GetColumnTypeCatalog;


public class GetColumnTypeCatalogResponse
{
    public IReadOnlyList<string> AllowedTypes { get; set; } = Array.Empty<string>();
    public IReadOnlyDictionary<string, string> TypeAliases { get; set; } = new Dictionary<string, string>();
}