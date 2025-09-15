using Application.Abstractions;
using Infrastructure.Files.Abstractions;

namespace Infrastructure.Files;

public sealed class DefaultColumnTypeValidator : IColumnTypeValidator
{
    private readonly ITypeCatalogService _typeCatalog;

    public DefaultColumnTypeValidator(ITypeCatalogService typeCatalog)
    {
        _typeCatalog = typeCatalog;
    }

    public IReadOnlyCollection<string> AllowedTypes => _typeCatalog.GetAllowedTypes();

    public bool TryNormalize(string? input, out string normalized)
    {
        return _typeCatalog.TryNormalize(input, out normalized);
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