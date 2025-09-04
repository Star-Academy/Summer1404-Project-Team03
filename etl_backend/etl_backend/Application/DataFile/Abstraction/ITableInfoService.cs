using etl_backend.Api.Dtos;

namespace etl_backend.Application.DataFile.Abstraction;

public interface ITableInfoService
{
    Task<TableDetailsDto> GetDetailsAsync(int schemaId, CancellationToken ct = default);
    Task<RowPreviewDto> PreviewRowsAsync(int schemaId, int offset, int limit, string? orderBy, string? direction, CancellationToken ct = default);
    Task<RowCountDto> GetRowCountAsync(int schemaId, bool exact, CancellationToken ct = default);
}
