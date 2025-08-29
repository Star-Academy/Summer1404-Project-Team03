using etl_backend.Application.DataFile.Dtos;

namespace etl_backend.Application.DataFile.Abstraction;

public interface IDataWrite
{
    Task<LoadResult> AppendRowsAsync(TableRef table, IRowSource rows, CancellationToken ct = default);
}