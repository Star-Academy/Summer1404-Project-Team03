namespace etl_backend.Application.DataFile.Abstraction;

public interface ICsvHeaderReader
{
    Task<IReadOnlyList<string>> ReadHeadersAsync(Stream stream, CancellationToken ct = default);
}

