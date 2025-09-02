using etl_backend.Application.DataFile.Abstraction;

namespace etl_backend.Application.DataFile.Services;

public sealed class DefaultColumnTypeValidator : IColumnTypeValidator
{
    // Canonical logical types
    private static readonly HashSet<string> Canonical =
        new(StringComparer.OrdinalIgnoreCase)
        { "string", "int", "long", "decimal", "bool", "datetime" };

    // Aliases map to canonical
    private static readonly Dictionary<string, string> Aliases =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["integer"] = "int",
            ["int32"] = "int",
            ["int64"] = "long",
            ["number"] = "decimal",
            ["numeric"] = "decimal",
            ["boolean"] = "bool",
            ["date"] = "datetime",
            ["datetimeoffset"] = "datetime",
            ["timestamp"] = "datetime",
            ["date-time"] = "datetime"
        };

    public IReadOnlyCollection<string> AllowedTypes => Canonical;

    public bool TryNormalize(string? input, out string normalized)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            normalized = "string";
            return true;
        }

        // direct canonical
        if (Canonical.Contains(input))
        {
            normalized = input.Trim().ToLowerInvariant();
            return true;
        }

        // alias
        if (Aliases.TryGetValue(input.Trim(), out var canon))
        {
            normalized = canon;
            return true;
        }

        normalized = string.Empty;
        return false;
    }

    public void ValidateOrThrow(IEnumerable<(int OrdinalPosition, string? Type)> items)
    {
        var invalid = new List<(int Ordinal, string? Type)>();
        foreach (var (ord, t) in items)
        {
            if (!TryNormalize(t, out _))
                invalid.Add((ord, t));
        }

        if (invalid.Count > 0)
        {
            var msg = "Invalid column types: " +
                      string.Join(", ", invalid.Select(x => $"#{x.Ordinal}='{x.Type}'"));
            throw new ArgumentException(msg);
        }
    }
}