using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Application.DataFile.Configurations;
using etl_backend.Domain.Entities;
using Microsoft.Extensions.Options;

namespace etl_backend.Application.DataFile.Services;

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