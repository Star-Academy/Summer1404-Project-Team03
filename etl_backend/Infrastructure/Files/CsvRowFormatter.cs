using etl_backend.Application.DataFile.Abstraction;

namespace etl_backend.Application.DataFile.Services;

public sealed class CsvRowFormatter : ICsvRowFormatter
{
    private readonly char _delimiter;
    private readonly char _quote;

    public CsvRowFormatter(char delimiter = ',', char quote = '"')
        => (_delimiter, _quote) = (delimiter, quote);

    public void WriteRow(TextWriter writer, string[] fields)
    {
        for (int i = 0; i < fields.Length; i++)
        {
            if (i > 0) writer.Write(_delimiter);
            var s = fields[i] ?? string.Empty;
            writer.Write(_quote);
            writer.Write(s.Replace(_quote.ToString(), new string(_quote, 2)));
            writer.Write(_quote);
        }
        writer.Write('\n');
    }
}