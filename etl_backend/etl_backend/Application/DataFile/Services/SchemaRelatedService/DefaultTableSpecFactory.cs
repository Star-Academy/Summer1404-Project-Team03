using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Application.DataFile.Dtos;
using etl_backend.Domain.Entities;

namespace etl_backend.Application.DataFile.Services;

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