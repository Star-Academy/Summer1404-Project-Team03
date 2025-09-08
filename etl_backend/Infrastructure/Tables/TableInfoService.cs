// using System.Data;
// using Application.Repositories.Abstractions;
// using etl_backend.Application.DataFile.Abstraction;
// using Infrastructure.Configurations;
// using Infrastructure.Files.Abstractions;
// using Microsoft.Extensions.Options;
// using Npgsql;
//
// namespace Infrastructure.Tables;
//
// public sealed class TableInfoService : ITableInfoService
// {
//     private readonly IDataTableSchemaRepository _schemas;
//     private readonly ITableCatalog _catalog;
//     private readonly IIdentifierPolicy _ids;
//     private readonly NpgsqlDataSource _ds;
//     private readonly string _defaultSchema;
//
//     public TableInfoService(
//         IDataTableSchemaRepository schemas,
//         ITableCatalog catalog,
//         IIdentifierPolicy ids,
//         NpgsqlDataSource dataSource,
//         IOptions<PostgresStoreOptions> store)
//     {
//         _schemas = schemas;
//         _catalog = catalog;
//         _ids = ids;
//         _ds = dataSource;
//         _defaultSchema = store.Value.DefaultSchema ?? "public";
//     }
//
//     public async Task<TableDetailsDto> GetDetailsAsync(int schemaId, CancellationToken ct = default)
//     {
//         var meta = await _schemas.GetByIdWithColumnsAsync(schemaId, ct)
//                    ?? throw new InvalidOperationException($"Schema {schemaId} not found.");
//
//         await using var conn = await _ds.OpenConnectionAsync(ct);
//         var npg = (NpgsqlConnection)conn;
//
//         var exists = await _catalog.TableExistsAsync(npg, _defaultSchema, meta.TableName, ct);
//
//         long approx = 0;
//         long size = 0;
//
//         if (exists)
//         {
//             approx = await GetApproxRowCountAsync(npg, _defaultSchema, meta.TableName, ct);
//             size = await GetTotalSizeAsync(npg, _defaultSchema, meta.TableName, ct);
//         }
//
//         var dto = new TableDetailsDto
//         {
//             SchemaId = meta.Id,
//             TableName = meta.TableName,
//             PhysicalExists = exists,
//             RowCountApprox = approx,
//             SizeBytes = size,
//             Columns = meta.Columns
//                 .OrderBy(c => c.OrdinalPosition)
//                 .Select(c => new ColumnDetailsDto { Ordinal = c.OrdinalPosition, Name = c.ColumnName, Type = c.ColumnType })
//                 .ToList()
//         };
//
//         return dto;
//     }
//
//     public async Task<RowPreviewDto> PreviewRowsAsync(int schemaId, int offset, int limit, string? orderBy, string? direction, CancellationToken ct = default)
//     {
//         if (limit <= 0) limit = 50;
//         if (limit > 200) limit = 200;
//         if (offset < 0) offset = 0;
//
//         var meta = await _schemas.GetByIdWithColumnsAsync(schemaId, ct)
//                    ?? throw new InvalidOperationException($"Schema {schemaId} not found.");
//
//         await using var conn = await _ds.OpenConnectionAsync(ct);
//         var npg = (NpgsqlConnection)conn;
//
//         if (!await _catalog.TableExistsAsync(npg, _defaultSchema, meta.TableName, ct))
//             throw new InvalidOperationException("Physical table does not exist.");
//
//         var cols = meta.Columns.OrderBy(c => c.OrdinalPosition).ToList();
//         var colList = string.Join(", ", cols.Select(c => $"{_ids.QuoteIdentifier(c.ColumnName)}"));
//         var qSchema = _ids.QuoteIdentifier(_defaultSchema);
//         var qTable = _ids.QuoteIdentifier(meta.TableName);
//
//         string orderClause = "";
//         if (!string.IsNullOrWhiteSpace(orderBy))
//         {
//             var found = cols.FirstOrDefault(c => string.Equals(c.ColumnName, orderBy, StringComparison.OrdinalIgnoreCase));
//             if (found != null)
//             {
//                 var dir = string.Equals(direction, "desc", StringComparison.OrdinalIgnoreCase) ? "DESC" : "ASC";
//                 orderClause = $" ORDER BY {_ids.QuoteIdentifier(found.ColumnName)} {dir}";
//             }
//         }
//
//         var sql = $"SELECT {colList} FROM {qSchema}.{qTable}{orderClause} LIMIT @limit OFFSET @offset;";
//         using var cmd = new NpgsqlCommand(sql, npg);
//         cmd.Parameters.AddWithValue("limit", NpgsqlTypes.NpgsqlDbType.Integer, limit);
//         cmd.Parameters.AddWithValue("offset", NpgsqlTypes.NpgsqlDbType.Integer, offset);
//         cmd.CommandType = CommandType.Text;
//         cmd.CommandTimeout = 30;
//
//         var rows = new List<Dictionary<string, object?>>(limit);
//
//         await using var reader = await cmd.ExecuteReaderAsync(ct);
//         while (await reader.ReadAsync(ct))
//         {
//             var dict = new Dictionary<string, object?>(reader.FieldCount, StringComparer.OrdinalIgnoreCase);
//             for (int i = 0; i < reader.FieldCount; i++)
//             {
//                 var name = reader.GetName(i);
//                 var val = reader.IsDBNull(i) ? null : reader.GetValue(i);
//                 dict[name] = val;
//             }
//             rows.Add(dict);
//         }
//
//         return new RowPreviewDto
//         {
//             Rows = rows,
//             NextOffset = offset + rows.Count
//         };
//     }
//
//     public async Task<RowCountDto> GetRowCountAsync(int schemaId, bool exact, CancellationToken ct = default)
//     {
//         var meta = await _schemas.GetByIdWithColumnsAsync(schemaId, ct)
//                    ?? throw new InvalidOperationException($"Schema {schemaId} not found.");
//
//         await using var conn = await _ds.OpenConnectionAsync(ct);
//         var npg = (NpgsqlConnection)conn;
//
//         if (!await _catalog.TableExistsAsync(npg, _defaultSchema, meta.TableName, ct))
//             throw new InvalidOperationException("Physical table does not exist.");
//
//         long count;
//         if (!exact)
//         {
//             count = await GetApproxRowCountAsync(npg, _defaultSchema, meta.TableName, ct);
//             return new RowCountDto { Exact = false, Count = count };
//         }
//
//         var qSchema = _ids.QuoteIdentifier(_defaultSchema);
//         var qTable = _ids.QuoteIdentifier(meta.TableName);
//         var sql = $"SELECT COUNT(*) FROM {qSchema}.{qTable};";
//
//         using var cmd = new NpgsqlCommand(sql, npg);
//         cmd.CommandTimeout = 0; // can be heavy; optionally cap
//         var result = await cmd.ExecuteScalarAsync(ct);
//         count = (result is long l) ? l : Convert.ToInt64(result);
//
//         return new RowCountDto { Exact = true, Count = count };
//     }
//
//     private static async Task<long> GetApproxRowCountAsync(NpgsqlConnection conn, string schema, string table, CancellationToken ct)
//     {
//         const string sql = @"
// SELECT COALESCE(c.reltuples::bigint, 0)
// FROM pg_class c
// JOIN pg_namespace n ON n.oid = c.relnamespace
// WHERE n.nspname = @schema AND c.relname = @table;";
//
//         using var cmd = new NpgsqlCommand(sql, conn);
//         cmd.Parameters.AddWithValue("schema", schema);
//         cmd.Parameters.AddWithValue("table", table);
//         var obj = await cmd.ExecuteScalarAsync(ct);
//         return obj is long l ? l : Convert.ToInt64(obj ?? 0);
//     }
//
//     private static async Task<long> GetTotalSizeAsync(NpgsqlConnection conn, string schema, string table, CancellationToken ct)
//     {
//         var regclass = $"\"{schema}\".\"{table}\"";
//         const string sql = @"SELECT COALESCE(pg_total_relation_size(@reg::regclass), 0);";
//         using var cmd = new NpgsqlCommand(sql, conn);
//         cmd.Parameters.AddWithValue("reg", regclass);
//         var obj = await cmd.ExecuteScalarAsync(ct);
//         return obj is long l ? l : Convert.ToInt64(obj ?? 0);
//     }
// }
