using Infrastructure.Files.Dtos;

namespace Infrastructure.Files.Abstractions;

public interface IDdlBuilder
{
    string BuildCreateTableSql(string qualifiedTableName, IReadOnlyList<ColumnSpec> columns, bool ifNotExists = false);
}