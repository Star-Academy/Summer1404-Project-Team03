using Application.Abstractions;
using Domain.Entities;
using Infrastructure.Files.Abstractions;

namespace Infrastructure.Files;

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