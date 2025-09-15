namespace etl_backend.Application.DataFile.Abstraction;

public interface ICsvRowFormatter
{
    void WriteRow(TextWriter writer, string[] fields);
}