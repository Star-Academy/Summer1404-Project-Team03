using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Domain.Entities;

namespace etl_backend.Application.DataFile.Services;

public sealed class DefaultColumnDefinitionBuilder : IColumnDefinitionBuilder
{
    private readonly IColumnNameSanitizer _sanitizer;

    public DefaultColumnDefinitionBuilder(IColumnNameSanitizer sanitizer) => _sanitizer = sanitizer;

    public List<DataTableColumn> Build(IReadOnlyList<string> headers)
    {
        var used = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var cols = new List<DataTableColumn>(headers.Count);

        for (int i = 0; i < headers.Count; i++)
        {
            var original = headers[i];
            var baseName = _sanitizer.Sanitize(original, i);

            var unique = baseName;
            int n = 2;
            while (!used.Add(unique)) unique = $"{baseName}_{n++}";

            cols.Add(new DataTableColumn
            {
                ColumnName = unique,
                OriginalColumnName = string.IsNullOrWhiteSpace(original) ? null : original,
                OrdinalPosition = i,
                ColumnType = "string"
            });
        }
        return cols;
    }
}