namespace etl_backend.Application.DataFile.Abstraction;

public interface IRowSource : IAsyncDisposable
{
    IAsyncEnumerable<string[]> ReadRowsAsync(CancellationToken ct = default);
}