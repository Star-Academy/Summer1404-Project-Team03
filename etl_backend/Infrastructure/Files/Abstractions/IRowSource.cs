namespace Infrastructure.Files.Abstractions;

public interface IRowSource : IAsyncDisposable
{
    IAsyncEnumerable<string[]> ReadRowsAsync(CancellationToken ct = default);
}