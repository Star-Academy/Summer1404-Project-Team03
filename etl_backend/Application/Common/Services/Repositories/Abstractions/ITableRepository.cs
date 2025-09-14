using Application.Dtos;
using Domain.Entities;

namespace Application.Services.Repositories.Abstractions;

public interface ITableRepository
{
    Task<bool> TableExistsAsync(string schemaName, string tableName, CancellationToken ct = default);
    Task<long> GetApproximateRowCountAsync(string schemaName, string tableName, CancellationToken ct = default);
    Task<long> GetTotalSizeAsync(string schemaName, string tableName, CancellationToken ct = default);
    Task<RowPreviewDto> PreviewRowsAsync(
        string schemaName,
        string tableName,
        List<DataTableColumn> columns,
        int offset,
        int limit,
        string? orderBy = null,
        string? direction = null,
        CancellationToken ct = default);
    Task<long> GetExactRowCountAsync(string schemaName, string tableName, CancellationToken ct = default);
}