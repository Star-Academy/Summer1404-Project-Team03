using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Application.DataFile.Dtos;

namespace etl_backend.Application.DataFile.Services;

using System.Diagnostics;
using etl_backend.Application.DataFile.Configurations;
using Microsoft.Extensions.Options;
using Npgsql;

public sealed class PostgresBulkWriter : IDataWrite
{
    private readonly NpgsqlDataSource _ds;
    private readonly ICsvRowFormatter _csvFmt;
    private readonly char _delimiter;
    private readonly char _quote;

    public PostgresBulkWriter(
        NpgsqlDataSource dataSource,
        ICsvRowFormatter csvFormatter,
        IOptions<CsvStagingOptions> csvOpts)
    {
        _ds        = dataSource;
        _csvFmt    = csvFormatter;
        _delimiter = csvOpts.Value.Delimiter;
        _quote     = csvOpts.Value.QuoteChar;
    }

    public async Task<LoadResult> AppendRowsAsync(TableRef table, IRowSource rows, CancellationToken ct = default)
    {
        var sw = Stopwatch.StartNew();
        long count = 0;

        var copySql = $"COPY {table.QualifiedName} FROM STDIN (FORMAT CSV, DELIMITER '{_delimiter}', QUOTE '{_quote}')";
        await using var conn = await _ds.OpenConnectionAsync(ct);
        await using (var writer = await ((NpgsqlConnection)conn).BeginTextImportAsync(copySql, ct))
        {
            await foreach (var row in rows.ReadRowsAsync(ct))
            {
                _csvFmt.WriteRow(writer, row);
                count++;
            }
        }

        sw.Stop();
        return new LoadResult(count, sw.Elapsed);
    }
}
