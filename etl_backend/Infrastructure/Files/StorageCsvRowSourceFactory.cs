using Domain.Entities;
using Infrastructure.Configurations;
using Infrastructure.Files.Abstractions;
using Microsoft.Extensions.Options;

namespace Infrastructure.Files;

public class StorageCsvRowSourceFactory : IRowSourceFactory
{
    private readonly IFileStorage _storage;
    private readonly CsvStagingOptions _opts;

    public StorageCsvRowSourceFactory(IFileStorage storage, IOptions<CsvStagingOptions> options)
        => (_storage, _opts) = (storage, options.Value);

    public async Task<IRowSource> CreateForStagedFileAsync(StagedFile staged, int expectedColumns, CancellationToken ct = default)
    {
        var stream = await _storage.OpenReadAsync(staged.StoredFilePath);
        return new CsvRowSource(stream, _opts, expectedColumns);
    }
}