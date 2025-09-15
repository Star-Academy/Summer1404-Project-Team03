using System.Text;

namespace Infrastructure.Configurations;

public enum CsvEncoding
{
    Utf8,
    Ascii,
    Latin1
}

public static class CsvEncodingExtensions
{
    public static Encoding ToSystemEncoding(this CsvEncoding encoding) =>
        encoding switch
        {
            CsvEncoding.Utf8 => Encoding.UTF8,
            CsvEncoding.Ascii => Encoding.ASCII,
            CsvEncoding.Latin1 => Encoding.Latin1,
            _ => throw new ArgumentOutOfRangeException(nameof(encoding))
        };
}
