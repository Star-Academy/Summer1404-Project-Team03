using etl_backend.Application.DataFile.Abstraction;

namespace etl_backend.Application.DataFile.Services;

using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using etl_backend.Application.DataFile.Configurations;
using etl_backend.Application.DataFile.Enums;
using Microsoft.Extensions.Options;


public sealed class CsvHeaderReader : ICsvHeaderReader
{
    private readonly CsvStagingOptions _opts;

    public CsvHeaderReader(IOptions<CsvStagingOptions> options)
        => _opts = options.Value;

    public async Task<IReadOnlyList<string>> ReadHeadersAsync(Stream stream, CancellationToken ct = default)
    {
        // Leave the underlying stream open; caller owns it.
        using var reader = new StreamReader(
            stream,
            _opts.Encoding.ToSystemEncoding(),
            detectEncodingFromByteOrderMarks: true,
            bufferSize: 1024,
            leaveOpen: true);

        var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = _opts.HasHeader,
            Delimiter = _opts.Delimiter.ToString(),
            Quote = _opts.QuoteChar,
            TrimOptions = _opts.TrimWhitespace ? TrimOptions.Trim : TrimOptions.None,
            BadDataFound = null,
            MissingFieldFound = null,
            DetectDelimiter = false
        };

        using var csv = new CsvReader(reader, cfg);

        // If file is empty
        if (!await csv.ReadAsync()) return Array.Empty<string>();

        if (_opts.HasHeader)
        {
            csv.ReadHeader();
            var headers = csv.HeaderRecord ?? Array.Empty<string>();
            return headers.ToList();
        }

        // No header: read first record to determine column count, then synthesize names.
        var record = csv.Parser.Record;
        var count = record?.Length ?? 0;
        if (count == 0) return Array.Empty<string>();

        var synthetic = new List<string>(count);
        for (int i = 1; i <= count; i++) synthetic.Add($"col_{i}");
        return synthetic;
    }
}
