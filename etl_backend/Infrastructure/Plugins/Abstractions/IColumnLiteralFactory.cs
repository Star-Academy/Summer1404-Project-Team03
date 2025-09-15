using Domain.Enums;
using Microsoft.Spark.Sql;

namespace Infrastructure.Plugins.Abstractions;

public interface IColumnLiteralFactory
{
    Column Create(string? raw, ValueTypeHint hint);
}