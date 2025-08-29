using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Domain.Entities;
using etl_backend.Domain.Enums;
using etl_backend.Repositories.Abstractions;

namespace etl_backend.Application.DataFile.Services.StageFileRelated;

public sealed class LoadPreconditionsService : ILoadPreconditionsService
{
    private readonly IStagedFileRepository _stagedRepo;
    private readonly IDataTableSchemaRepository _schemaRepo;

    public LoadPreconditionsService(IStagedFileRepository stagedRepo, IDataTableSchemaRepository schemaRepo)
        => (_stagedRepo, _schemaRepo) = (stagedRepo, schemaRepo);

    public async Task<(StagedFile staged, DataTableSchema schema)> EnsureLoadableAsync(int stagedFileId, CancellationToken ct = default)
    {
        var staged = await _stagedRepo.GetByIdAsync(stagedFileId, ct)
                     ?? throw new InvalidOperationException($"Staged file {stagedFileId} not found.");

        if (staged.Status == ProcessingStatus.Failed)
            throw new InvalidOperationException("Staged file is in Failed state.");

        if (staged.Stage != ProcessingStage.SchemaRegistered)
            throw new InvalidOperationException("Staged file is not ready to load (expecting SchemaRegistered).");

        var schemaId = staged.SchemaId ?? throw new InvalidOperationException("Staged file has no SchemaId.");

        var schema = await _schemaRepo.GetByIdWithColumnsAsync(schemaId, ct)
                     ?? throw new InvalidOperationException($"Schema {schemaId} not found.");

        if (schema.Columns is null || schema.Columns.Count == 0)
            throw new InvalidOperationException("Schema has no columns.");

        return (staged, schema);
    }
}