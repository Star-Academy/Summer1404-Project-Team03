using System.Data;
using Application.Repositories.Abstractions;
using Application.ValueObjects;
using Domain.Entities;
using etl_backend.Application.DataFile.Abstraction;
using Infrastructure.Configurations;
using Infrastructure.Files.Abstractions;
using Infrastructure.Tables.Abstractions;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Infrastructure.Repositories;

public sealed class TableRepository : ITableRepository
{
    private readonly INpgsqlDataSourceFactory _dataSourceFactory;
    private readonly ITableCatalog _tableCatalog;
    private readonly IIdentifierPolicy _identifierPolicy;
    private readonly string _defaultSchema;

    public TableRepository(
        INpgsqlDataSourceFactory dataSourceFactory,
        ITableCatalog tableCatalog,
        IIdentifierPolicy identifierPolicy,
        IOptions<PostgresStoreOptions> storeOptions)
    {
        _dataSourceFactory = dataSourceFactory;
        _tableCatalog = tableCatalog;
        _identifierPolicy = identifierPolicy;
        _defaultSchema = storeOptions.Value.DefaultSchema ?? "public";
    }

    public async Task<bool> TableExistsAsync(string schemaName, string tableName, CancellationToken ct = default)
    {
        await using var conn = await _dataSourceFactory.CreateConnectionAsync(ct);
        return await _tableCatalog.TableExistsAsync(conn, schemaName, tableName, ct);
    }

    public async Task<long> GetApproximateRowCountAsync(string schemaName, string tableName, CancellationToken ct = default)
    {
        await using var conn = await _dataSourceFactory.CreateConnectionAsync(ct);
        const string sql = @"
            SELECT COALESCE(c.reltuples::bigint, 0)
            FROM pg_class c
            JOIN pg_namespace n ON n.oid = c.relnamespace
            WHERE n.nspname = @schema AND c.relname = @table;";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("schema", schemaName);
        cmd.Parameters.AddWithValue("table", tableName);
        var result = await cmd.ExecuteScalarAsync(ct);
        return result is long l ? l : Convert.ToInt64(result ?? 0);
    }

    public async Task<long> GetTotalSizeAsync(string schemaName, string tableName, CancellationToken ct = default)
    {
        await using var conn = await _dataSourceFactory.CreateConnectionAsync(ct);
        var regclass = $"\"{schemaName}\".\"{tableName}\"";
        const string sql = @"SELECT COALESCE(pg_total_relation_size(@reg::regclass), 0);";
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("reg", regclass);
        var result = await cmd.ExecuteScalarAsync(ct);
        return result is long l ? l : Convert.ToInt64(result ?? 0);
    }

    public async Task<RowPreviewDto> PreviewRowsAsync(
        string schemaName,
        string tableName,
        List<DataTableColumn> columns,
        int offset,
        int limit,
        string? orderBy = null,
        string? direction = null,
        CancellationToken ct = default)
    {
        await using var conn = await _dataSourceFactory.CreateConnectionAsync(ct);

        var colList = string.Join(", ", columns.Select(c => $"{_identifierPolicy.QuoteIdentifier(c.ColumnName)}"));
        var qSchema = _identifierPolicy.QuoteIdentifier(schemaName);
        var qTable = _identifierPolicy.QuoteIdentifier(tableName);

        string orderClause = "";
        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            var found = columns.FirstOrDefault(c => string.Equals(c.ColumnName, orderBy, StringComparison.OrdinalIgnoreCase));
            if (found != null)
            {
                var dir = string.Equals(direction, "desc", StringComparison.OrdinalIgnoreCase) ? "DESC" : "ASC";
                orderClause = $" ORDER BY {_identifierPolicy.QuoteIdentifier(found.ColumnName)} {dir}";
            }
        }

        var sql = $"SELECT {colList} FROM {qSchema}.{qTable}{orderClause} LIMIT @limit OFFSET @offset;";
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("limit", NpgsqlTypes.NpgsqlDbType.Integer, limit);
        cmd.Parameters.AddWithValue("offset", NpgsqlTypes.NpgsqlDbType.Integer, offset);
        cmd.CommandType = CommandType.Text;
        cmd.CommandTimeout = 30;

        var rows = new List<Dictionary<string, object?>>(limit);
        await using var reader = await cmd.ExecuteReaderAsync(ct);

        while (await reader.ReadAsync(ct))
        {
            var dict = new Dictionary<string, object?>(reader.FieldCount, StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var name = reader.GetName(i);
                var val = reader.IsDBNull(i) ? null : reader.GetValue(i);
                dict[name] = val;
            }
            rows.Add(dict);
        }

        return new RowPreviewDto
        {
            Rows = rows,
            NextOffset = offset + rows.Count
        };
    }

    public async Task<long> GetExactRowCountAsync(string schemaName, string tableName, CancellationToken ct = default)
    {
        await using var conn = await _dataSourceFactory.CreateConnectionAsync(ct);
        var qSchema = _identifierPolicy.QuoteIdentifier(schemaName);
        var qTable = _identifierPolicy.QuoteIdentifier(tableName);
        var sql = $"SELECT COUNT(*) FROM {qSchema}.{qTable};";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.CommandTimeout = 0;
        var result = await cmd.ExecuteScalarAsync(ct);
        return result is long l ? l : Convert.ToInt64(result);
    }
}