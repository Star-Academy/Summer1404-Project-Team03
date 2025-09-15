using Application.Services.Repositories.Abstractions;
using etl_backend.Application.DataFile.Abstraction;
using Infrastructure.Files.Abstractions;
using Npgsql;

namespace Infrastructure.Repositories;


public sealed class ColumnRepository : IColumnRepository
{
    private readonly IColumnAdmin _columnAdmin;
    private readonly ITableCatalog _tableCatalog;
    private readonly NpgsqlDataSource _dataSource;
    private readonly string _defaultSchema;

    public ColumnRepository(
        IColumnAdmin columnAdmin,
        ITableCatalog tableCatalog,
        NpgsqlDataSource dataSource,
        string defaultSchema = "public")
    {
        _columnAdmin = columnAdmin;
        _tableCatalog = tableCatalog;
        _dataSource = dataSource;
        _defaultSchema = defaultSchema;
    }

    public async Task<bool> TableExistsAsync(string schemaName, string tableName, CancellationToken ct = default)
    {
        await using var conn = await _dataSource.OpenConnectionAsync(ct);
        return await _tableCatalog.TableExistsAsync(conn, schemaName, tableName, ct);
    }

    public async Task RenameColumnAsync(string schemaName, string tableName, string oldName, string newName, CancellationToken ct = default)
    {
        await using var conn = await _dataSource.OpenConnectionAsync(ct);
        await _columnAdmin.RenameAsync(conn, schemaName, tableName, oldName, newName, ct);
    }

    public async Task DropColumnsAsync(string schemaName, string tableName, List<string> columnNames, CancellationToken ct = default)
    {
        await using var conn = await _dataSource.OpenConnectionAsync(ct);
        await _columnAdmin.DropAsync(conn, schemaName, tableName, columnNames, ct);
    }
}