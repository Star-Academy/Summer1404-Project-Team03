using etl_backend.Repositories.Abstractions;

namespace etl_backend.Application.DataFile.Services.StageFileRelated;

using etl_backend.Domain.Entities;
using etl_backend.Domain.Enums; 

public sealed class StagedFileStateService : IStagedFileStateService
{
    private readonly IStagedFileRepository _repo;
    public StagedFileStateService(IStagedFileRepository repo) => _repo = repo;

    public async Task<StagedFile> MarkSchemaRegisteredAsync(StagedFile s, int schemaId, CancellationToken ct = default)
    {
        s.SchemaId = schemaId;
        s.Stage = ProcessingStage.SchemaRegistered;
        s.Status = ProcessingStatus.InProgress;
        s.ErrorCode = ProcessingErrorCode.None;
        s.ErrorMessage = null;
        await _repo.UpdateAsync(s, ct);
        return s;
    }

    public async Task<StagedFile> MarkTableCreatedAsync(StagedFile s, CancellationToken ct = default)
    {
        s.Stage = ProcessingStage.TableCreated;
        s.Status = ProcessingStatus.InProgress;
        s.ErrorCode = ProcessingErrorCode.None;
        s.ErrorMessage = null;
        await _repo.UpdateAsync(s, ct);
        return s;
    }

    public async Task<StagedFile> MarkLoadedSucceededAsync(StagedFile s, CancellationToken ct = default)
    {
        s.Stage = ProcessingStage.Loaded;
        s.Status = ProcessingStatus.Succeeded;
        s.ErrorCode = ProcessingErrorCode.None;
        s.ErrorMessage = null;
        await _repo.UpdateAsync(s, ct);
        return s;
    }

    public async Task<StagedFile> FailSchemaRegistrationAsync(StagedFile s, string error, CancellationToken ct = default)
    {
        s.Status = ProcessingStatus.Failed;
        s.ErrorCode = ProcessingErrorCode.SchemaRegistrationFailed;
        s.ErrorMessage = error;
        await _repo.UpdateAsync(s, ct);
        return s;
    }

    public async Task<StagedFile> FailSchemaDbWriteAsync(StagedFile s, string error, CancellationToken ct = default)
    {
        s.Status = ProcessingStatus.Failed;
        s.ErrorCode = ProcessingErrorCode.SchemaDbWriteFailed;
        s.ErrorMessage = error;
        await _repo.UpdateAsync(s, ct);
        return s;
    }

    public async Task<StagedFile> FailCreateTableAsync(StagedFile s, string error, CancellationToken ct = default)
    {
        s.Status = ProcessingStatus.Failed;
        s.ErrorCode = ProcessingErrorCode.CreateTableFailed;
        s.ErrorMessage = error;
        await _repo.UpdateAsync(s, ct);
        return s;
    }

    public async Task<StagedFile> FailLoadAsync(StagedFile s, string error, CancellationToken ct = default)
    {
        s.Status = ProcessingStatus.Failed;
        s.ErrorCode = ProcessingErrorCode.LoadFailed;
        s.ErrorMessage = error;
        await _repo.UpdateAsync(s, ct);
        return s;
    }
}
