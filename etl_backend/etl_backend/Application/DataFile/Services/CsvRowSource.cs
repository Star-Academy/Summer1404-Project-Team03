using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Application.DataFile.Configurations;
using etl_backend.Application.DataFile.Enums;

namespace etl_backend.Application.DataFile.Services;

using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;


public sealed class CsvRowSource : IRowSource
{
    private readonly Stream _stream;
    private readonly StreamReader _reader;
    private readonly CsvReader _csv;
    private readonly int _expectedCols;
    private bool _initialized;

    public CsvRowSource(Stream stream, CsvStagingOptions opts, int expectedColumns)
    {
        _stream = stream;
        _reader = new StreamReader(
            _stream,
            opts.Encoding.ToSystemEncoding(),
            detectEncodingFromByteOrderMarks: true,
            bufferSize: 1 << 12,
            leaveOpen: false);

        var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = opts.HasHeader,
            Delimiter = opts.Delimiter.ToString(),
            Quote = opts.QuoteChar,
            TrimOptions = opts.TrimWhitespace ? TrimOptions.Trim : TrimOptions.None,
            BadDataFound = null,
            MissingFieldFound = null,
            DetectDelimiter = false
        };

        _csv = new CsvReader(_reader, cfg);
        _expectedCols = expectedColumns;
    }

    public async IAsyncEnumerable<string[]> ReadRowsAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
    {
        if (!_initialized)
        {
            if (_csv.Configuration.HasHeaderRecord)
            {
                if (!await _csv.ReadAsync()) yield break; // empty file
                _csv.ReadHeader(); // skip header row
            }
            _initialized = true;
        }

        while (await _csv.ReadAsync())
        {
            ct.ThrowIfCancellationRequested();

            // Build row with a fixed number of columns (pad/truncate)
            var row = new string[_expectedCols];
            for (int i = 0; i < _expectedCols; i++)
            {
                string? v;
                try { v = _csv.GetField(i); }
                catch { v = null; } // missing field -> null
                row[i] = v ?? string.Empty; // keep empty for now; provider may map empty->NULL
            }
            yield return row;
        }
    }

    public ValueTask DisposeAsync()
    {
        _csv.Dispose();
        _reader.Dispose();
        _stream.Dispose();
        return ValueTask.CompletedTask;
    }
}
