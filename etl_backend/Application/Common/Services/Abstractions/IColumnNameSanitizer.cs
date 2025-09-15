namespace Application.Abstractions;

public interface IColumnNameSanitizer
{
    string Sanitize(string? raw, int index);
}