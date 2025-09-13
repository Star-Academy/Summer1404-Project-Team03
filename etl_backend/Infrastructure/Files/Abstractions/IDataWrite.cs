using Application.Dtos;
using Infrastructure.Files.Dtos;

namespace Infrastructure.Files.Abstractions;

public interface IDataWrite
{
    Task<LoadResult> AppendRowsAsync(TableRef table, IRowSource rows, CancellationToken ct = default);
}