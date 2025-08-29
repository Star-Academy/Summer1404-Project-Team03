using etl_backend.Application.DataFile.Abstraction;

namespace etl_backend.Application.DataFile.Services;

public sealed class PostgresColumnNameSanitizer : IColumnNameSanitizer
{
    public string Sanitize(string? raw, int index)
    {
        if (string.IsNullOrWhiteSpace(raw)) return $"col_{index + 1}";
        var s = new string(raw.Trim().Select(ch => char.IsLetterOrDigit(ch) ? ch : '_').ToArray());
        if (s.Length == 0) s = $"col_{index + 1}";
        if (char.IsDigit(s[0])) s = "c_" + s;
        return s.ToLowerInvariant();
    }
}