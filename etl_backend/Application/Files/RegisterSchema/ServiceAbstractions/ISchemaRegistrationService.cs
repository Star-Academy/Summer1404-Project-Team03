using Domain.Entities;

namespace Application.Abstractions;

public interface ISchemaRegistrationService
{
    Task<(DataTableSchema Schema, StagedFile Staged)> RegisterAsync(
        Guid stagedFileId,
        IReadOnlyDictionary<int, string> columnTypesByOrdinal,
        IReadOnlyDictionary<int, string> columnNamesByOrdinal,
        CancellationToken ct = default);
}