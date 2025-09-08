using Domain.Entities;
using Infrastructure.Files.Abstractions;
using Infrastructure.Files.Dtos;

namespace Infrastructure.Files;

public sealed class DefaultTableSpecFactory : ITableSpecFactory
{
    public TableSpec From(DataTableSchema schema)
    {
        var cols = schema.Columns
            .OrderBy(c => c.OrdinalPosition)
            .Select(c => new ColumnSpec(c.ColumnName, c.ColumnType)) // today: "string"
            .ToList();

        return new TableSpec(schema.TableName, cols);
    }
}