using Application.Services.Repositories.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Files.Abstractions;

namespace Infrastructure.Files;

public sealed class LoadPreconditionsService : ILoadPreconditionsService
{
    private readonly IStagedFileRepository _stagedRepo;
    private readonly IDataTableSchemaRepository _schemaRepo;

    public LoadPreconditionsService(IStagedFileRepository stagedRepo, IDataTableSchemaRepository schemaRepo)
        => (_stagedRepo, _schemaRepo) = (stagedRepo, schemaRepo);

    public async Task<(StagedFile staged, DataTableSchema schema)> EnsureLoadableAsync(Guid stagedFileId, CancellationToken ct = default)
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