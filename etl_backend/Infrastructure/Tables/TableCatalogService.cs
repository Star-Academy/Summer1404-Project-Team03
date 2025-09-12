using Application.Abstractions;
using Application.Common.Configurations;
using Microsoft.Extensions.Options;

namespace Infrastructure.Tables;



public class TypeCatalogService : ITypeCatalogService
{
    private readonly ColumnTypeConfiguration _config;

    public TypeCatalogService(IOptions<ColumnTypeConfiguration> config)
    {
        _config = config.Value;
    }

    public IReadOnlyList<string> GetAllowedTypes()
    {
        return _config.CanonicalTypes.AsReadOnly();
    }

    public IReadOnlyDictionary<string, string> GetTypeAliases()
    {
        return new Dictionary<string, string>(_config.TypeAliases, StringComparer.OrdinalIgnoreCase);
    }

    public bool TryNormalize(string input, out string normalized)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            normalized = "string";
            return true;
        }

        var trimmed = input.Trim();

        // Direct match in canonical types
        if (_config.CanonicalTypes.Contains(trimmed, StringComparer.OrdinalIgnoreCase))
        {
            normalized = trimmed.ToLowerInvariant();
            return true;
        }

        // Check aliases
        if (_config.TypeAliases.TryGetValue(trimmed, out var canon))
        {
            normalized = canon;
            return true;
        }

        normalized = string.Empty;
        return false;
    }
}