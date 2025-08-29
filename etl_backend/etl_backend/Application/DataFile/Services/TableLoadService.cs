using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Application.DataFile.Dtos;
using etl_backend.Application.DataFile.Enums;
using etl_backend.Application.DataFile.Services.StageFileRelated;
namespace etl_backend.Application.DataFile.Services;

public sealed class TableLoadService : ITableLoadService
{
    private readonly ILoadPreconditionsService _pre;
    private readonly ITableSpecFactory _specFactory;
    private readonly ITableAdmin _tableAdmin;
    private readonly IDataWrite _writer;
    private readonly IRowSourceFactory _rows;
    private readonly IStagedFileStateService _state;

    public TableLoadService(
        ILoadPreconditionsService preconditions,
        ITableSpecFactory specFactory,
        ITableAdmin tableAdmin,
        IDataWrite writer,
        IRowSourceFactory rows,
        IStagedFileStateService state)
        => (_pre, _specFactory, _tableAdmin, _writer, _rows, _state)
         = (preconditions, specFactory, tableAdmin, writer, rows, state);

    public async Task<LoadResult> LoadAsync(int stagedFileId, ILoadPolicy policy, CancellationToken ct = default)
    {
        // Preconditions: fetch staged + schema and validate readiness
        var (staged, schema) = await _pre.EnsureLoadableAsync(stagedFileId, ct);
        // Ensure table (DDL)
        TableRef tableRef;
        try
        {
            var spec = _specFactory.From(schema);
            tableRef = await _tableAdmin.EnsureTableAsync(spec, policy.Mode, ct);
            await _state.MarkTableCreatedAsync(staged, ct);
        }
        catch (Exception ex)
        {
            await _state.FailCreateTableAsync(staged, ex.Message, ct);
            throw;
        }
        // Append rows (DML)
        try
        {
            await using var rowSource = await _rows.CreateForStagedFileAsync(staged, schema.Columns.Count, ct);
            var result = await _writer.AppendRowsAsync(tableRef, rowSource, ct);

            await _state.MarkLoadedSucceededAsync(staged, ct);
            return result;
        }
        catch (Exception ex)
        {
            await _state.FailLoadAsync(staged, ex.Message, ct);
            if (policy.DropOnFailure)
            {
                try
                {
                    if (policy.Mode == LoadMode.Replace)
                        await _tableAdmin.DropTableAsync(tableRef, ct);
                    else if (policy.Mode == LoadMode.Append)
                        await _tableAdmin.TruncateTableAsync(tableRef, ct);
                }
                catch { /* best-effort cleanup */ }
            }

            throw;
        }
    }
}
