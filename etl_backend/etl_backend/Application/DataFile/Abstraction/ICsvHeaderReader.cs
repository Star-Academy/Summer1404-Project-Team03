namespace etl_backend.Application.DataFile.Abstraction;

public interface ICsvHeaderReader
{
    Task<IReadOnlyList<string>> ReadHeadersAsync(Stream stream, char delimiter = ',', CancellationToken ct = default);
}
