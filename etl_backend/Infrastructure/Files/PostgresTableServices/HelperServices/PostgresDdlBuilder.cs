using System.Text;
using etl_backend.Application.DataFile.Abstraction;
using Infrastructure.Files.Abstractions;
using Infrastructure.Files.Dtos;

namespace Infrastructure.Files.PostgresTableServices.HelperServices;

public sealed class PostgresDdlBuilder : IDdlBuilder
{
    private readonly IIdentifierPolicy _ids;
    private readonly ITypeMapper _types;

    public PostgresDdlBuilder(IIdentifierPolicy ids, ITypeMapper types)
        => (_ids, _types) = (ids, types);

    public string BuildCreateTableSql(string qualifiedTableName, IReadOnlyList<ColumnSpec> columns, bool ifNotExists = false)
    {
        var sb = new StringBuilder();
        sb.Append("CREATE TABLE ");
        if (ifNotExists) sb.Append("IF NOT EXISTS ");
        sb.Append(qualifiedTableName).Append(" (");

        for (int i = 0; i < columns.Count; i++)
        {
            if (i > 0) sb.Append(", ");
            var c = columns[i];
            sb.Append(_ids.QuoteIdentifier(c.Name))
                .Append(' ')
                .Append(_types.ToProviderType(c.LogicalType));
        }
        sb.Append(");");
        return sb.ToString();
    }
}