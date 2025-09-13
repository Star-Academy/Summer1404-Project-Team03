using System.Collections.ObjectModel;
using Application.Abstractions;
using Application.Services.Repositories.Abstractions;
using Domain.Entities;
using etl_backend.Application.DataFile.Abstraction;
using Infrastructure.Configurations;
using Infrastructure.Files.Abstractions;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Infrastructure.Tables;

public sealed class ColumnManagementService : IColumnManagementService
{
    private readonly IDataTableSchemaRepository _schemas;
    private readonly IDataTableColumnRepository _columns;
    private readonly IColumnNameSanitizer _sanitizer;
    private readonly IIdentifierPolicy _ids;
    private readonly ITableCatalog _catalog;
    private readonly IColumnAdmin _ddl;
    private readonly NpgsqlDataSource _ds;
    private readonly string _defaultSchema;

    public ColumnManagementService(
        IDataTableSchemaRepository schemas,
        IDataTableColumnRepository columns,
        IColumnNameSanitizer sanitizer,
        IIdentifierPolicy ids,
        ITableCatalog catalog,
        IColumnAdmin ddl,
        NpgsqlDataSource ds,
        IOptions<PostgresStoreOptions> store)
    {
        _schemas = schemas;
        _columns = columns;
        _sanitizer = sanitizer;
        _ids = ids;
        _catalog = catalog;
        _ddl = ddl;
        _ds = ds;
        _defaultSchema = store.Value.DefaultSchema ?? "public";
    }

    public async Task<IReadOnlyList<DataTableColumn>> ListAsync(int schemaId, CancellationToken ct = default)
    {
        var schema = await _schemas.GetByIdWithColumnsAsync(schemaId, ct)
                    ?? throw new InvalidOperationException($"Schema {schemaId} not found.");

        var ordered = schema.Columns
            .OrderBy(c => c.OrdinalPosition)
            .ToList();

        return new ReadOnlyCollection<DataTableColumn>(ordered);
    }

    public async Task RenameAsync(int schemaId, int columnId, string newName, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("New column name is required.", nameof(newName));

        // Postgres identifier max length is typically 63; prefer policy if exposed
        var maxLen = (_ids as IHasMaxIdentifierLength)?.MaxIdentifierLength ?? 63;
        newName = _sanitizer.Sanitize(newName.Trim(), maxLen);
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Sanitized name is empty.", nameof(newName));

        var schema = await _schemas.GetByIdWithColumnsAsync(schemaId, ct)
                    ?? throw new InvalidOperationException($"Schema {schemaId} not found.");

        var col = schema.Columns.FirstOrDefault(c => c.Id == columnId)
                  ?? throw new InvalidOperationException($"Column {columnId} not found.");

        if (schema.Columns.Any(c => c.Id != columnId &&
            string.Equals(c.ColumnName, newName, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException($"A column with name '{newName}' already exists in this table.");

        await using var conn = await _ds.OpenConnectionAsync(ct);
        var npg = (NpgsqlConnection)conn;

        var exists = await _catalog.TableExistsAsync(npg, _defaultSchema, schema.TableName, ct);
        if (!exists)
            throw new InvalidOperationException("Physical table does not exist.");

        var oldName = col.ColumnName;

        try
        {
            await _ddl.RenameAsync(npg, _defaultSchema, schema.TableName, oldName, newName, ct);
        }
        catch
        {
            throw;
        }

        try
        {
            await _columns.UpdateNameAsync(columnId, newName, ct);
        }
        catch
        {
            try { await _ddl.RenameAsync(npg, _defaultSchema, schema.TableName, newName, oldName, ct); } catch { /* best-effort */ }
            throw;
        }
    }

    public async Task DropAsync(int schemaId, IReadOnlyCollection<int> columnIds, CancellationToken ct = default)
    {
        if (columnIds == null || columnIds.Count == 0)
            return;

        var schema = await _schemas.GetByIdWithColumnsAsync(schemaId, ct)
                    ?? throw new InvalidOperationException($"Schema {schemaId} not found.");

        var existingIds = schema.Columns.Select(c => c.Id).ToHashSet();
        var notFound = columnIds.Where(id => !existingIds.Contains(id)).ToList();
        if (notFound.Count > 0)
            throw new ArgumentException("Some columns were not found: " + string.Join(", ", notFound));

        var remaining = schema.Columns.Count - columnIds.Count;
        if (remaining < 1)
            throw new InvalidOperationException("Cannot drop all columns. A table must have at least one column.");

        var namesToDrop = schema.Columns
            .Where(c => columnIds.Contains(c.Id))
            .Select(c => c.ColumnName)
            .ToList();

        await using var conn = await _ds.OpenConnectionAsync(ct);
        var npg = (NpgsqlConnection)conn;

        var exists = await _catalog.TableExistsAsync(npg, _defaultSchema, schema.TableName, ct);
        if (!exists)
            throw new InvalidOperationException("Physical table does not exist.");

        try
        {
            await _ddl.DropAsync(npg, _defaultSchema, schema.TableName, namesToDrop, ct);
        }
        catch
        {
            throw;
        }

        await _columns.DeleteByIdsAsync(columnIds, ct);
    }

    private interface IHasMaxIdentifierLength
    {
        int MaxIdentifierLength { get; }
    }
}