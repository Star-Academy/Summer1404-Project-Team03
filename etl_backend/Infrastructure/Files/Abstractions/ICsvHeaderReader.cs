namespace Infrastructure.Files.Abstractions;

public interface ICsvHeaderReader
{
    Task<IReadOnlyList<string>> ReadHeadersAsync(Stream stream, CancellationToken ct = default);
}

