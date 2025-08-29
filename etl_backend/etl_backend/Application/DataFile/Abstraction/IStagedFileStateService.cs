namespace etl_backend.Application.DataFile.Services.StageFileRelated;

using etl_backend.Domain.Entities;

public interface IStagedFileStateService
{
    Task<StagedFile> MarkSchemaRegisteredAsync(StagedFile staged, int schemaId, CancellationToken ct = default);
    Task<StagedFile> MarkTableCreatedAsync(StagedFile staged, CancellationToken ct = default);
    Task<StagedFile> MarkLoadedSucceededAsync(StagedFile staged, CancellationToken ct = default);

    Task<StagedFile> FailSchemaRegistrationAsync(StagedFile staged, string error, CancellationToken ct = default);
    Task<StagedFile> FailSchemaDbWriteAsync(StagedFile staged, string error, CancellationToken ct = default);
    Task<StagedFile> FailCreateTableAsync(StagedFile staged, string error, CancellationToken ct = default);
    Task<StagedFile> FailLoadAsync(StagedFile staged, string error, CancellationToken ct = default);
}
