using Domain.Entities;

namespace Application.Abstractions;

public interface ISchemaRegistrationService
{
    Task<(DataTableSchema Schema, StagedFile Staged)> RegisterAsync(int stagedFileId, CancellationToken ct = default);
    
    Task<(DataTableSchema Schema, StagedFile Staged)> RegisterAsync(
        int stagedFileId,
        IReadOnlyDictionary<int, string> columnTypesByOrdinal,
        CancellationToken ct = default);
}