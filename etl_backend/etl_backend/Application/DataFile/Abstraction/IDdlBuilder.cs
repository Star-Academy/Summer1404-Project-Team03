using etl_backend.Application.DataFile.Dtos;

namespace etl_backend.Application.DataFile.Abstraction;

public interface IDdlBuilder
{
    string BuildCreateTableSql(string qualifiedTableName, IReadOnlyList<ColumnSpec> columns, bool ifNotExists = false);
}