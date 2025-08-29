using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Domain.Entities;

namespace etl_backend.Application.DataFile.Services;

public sealed class StorageHeaderProvider : IHeaderProvider
{
    private readonly IFileStorage _storage;
    private readonly ICsvHeaderReader _reader;

    public StorageHeaderProvider(IFileStorage storage, ICsvHeaderReader reader)
        => (_storage, _reader) = (storage, reader);

    public async Task<IReadOnlyList<string>> GetAsync(StagedFile staged, CancellationToken ct = default)
    {
        await using var stream = await _storage.OpenReadAsync(staged.StoredFilePath);
        return await _reader.ReadHeadersAsync(stream, ct);
    }
}