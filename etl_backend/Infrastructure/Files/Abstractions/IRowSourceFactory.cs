using Domain.Entities;

namespace Infrastructure.Files.Abstractions;

public interface IRowSourceFactory
{
    Task<IRowSource> CreateForStagedFileAsync(StagedFile staged, int expectedColumns, CancellationToken ct = default);
}