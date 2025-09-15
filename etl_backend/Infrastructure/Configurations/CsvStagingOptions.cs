namespace Infrastructure.Configurations;

public class CsvStagingOptions
{
    /// <summary>
    /// Character used to separate values (e.g. ',', ';', '\t').
    /// </summary>
    public char Delimiter { get; set; } = ',';

    /// <summary>
    /// Character used to quote values that contain delimiters.
    /// </summary>
    public char QuoteChar { get; set; } = '"';

    /// <summary>
    /// Whether the first row contains column headers.
    /// </summary>
    public bool HasHeader { get; set; } = true;

    /// <summary>
    /// Whether to trim whitespace around values.
    /// </summary>
    public bool TrimWhitespace { get; set; } = true;

    /// <summary>
    /// Encoding of the CSV file (default UTF-8).
    /// </summary>
    public CsvEncoding Encoding { get; set; } = CsvEncoding.Utf8;
    
}

