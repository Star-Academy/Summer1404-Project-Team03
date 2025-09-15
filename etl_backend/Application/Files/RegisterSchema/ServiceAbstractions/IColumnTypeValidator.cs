namespace Infrastructure.Files.Abstractions;

public interface IColumnTypeValidator
{
    bool TryNormalize(string? input, out string normalized);
    void ValidateOrThrow(IEnumerable<(int OrdinalPosition, string? Type)> items);
    IReadOnlyCollection<string> AllowedTypes { get; }
}