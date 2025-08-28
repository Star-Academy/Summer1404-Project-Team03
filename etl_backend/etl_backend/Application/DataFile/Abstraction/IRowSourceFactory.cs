using etl_backend.Domain.Entities;

namespace etl_backend.Application.DataFile.Abstraction;

public interface IRowSourceFactory
{
    Task<IRowSource> CreateForStagedFileAsync(StagedFile staged, int expectedColumns, CancellationToken ct = default);
}