namespace etl_backend.Application.DataFile.Abstraction;

public interface IColumnNameSanitizer
{
    string Sanitize(string? raw, int index);
}