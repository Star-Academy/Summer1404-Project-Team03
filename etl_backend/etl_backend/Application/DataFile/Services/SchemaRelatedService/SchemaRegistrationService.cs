using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Domain.Entities;
using etl_backend.Domain.Enums;
using etl_backend.Repositories.Abstractions;

namespace etl_backend.Application.DataFile.Services;

public sealed class SchemaRegistrationService : ISchemaRegistrationService
{
    private readonly IStagedFileRepository _stagedRepo;
    private readonly IDataTableSchemaRepository _schemaRepo;
    private readonly IHeaderProvider _headers;
    private readonly IColumnDefinitionBuilder _cols;
    private readonly ITableNameGenerator _names;

    public SchemaRegistrationService(
        IStagedFileRepository stagedRepo,
        IDataTableSchemaRepository schemaRepo,
        IHeaderProvider headers,
        IColumnDefinitionBuilder cols,
        ITableNameGenerator names)
        => (_stagedRepo, _schemaRepo, _headers, _cols, _names) = (stagedRepo, schemaRepo, headers, cols, names);

    public async Task<(DataTableSchema Schema, StagedFile Staged)> RegisterAsync(int stagedFileId, CancellationToken ct = default)
    {
        var staged = await _stagedRepo.GetByIdAsync(stagedFileId, ct)
                    ?? throw new InvalidOperationException($"Staged file {stagedFileId} not found.");

        if (staged.Stage != ProcessingStage.Uploaded || staged.Status == ProcessingStatus.Failed)
            throw new InvalidOperationException($"Staged file {stagedFileId} not in a registrable state.");

        IReadOnlyList<string> headers;
        try
        {
            headers = await _headers.GetAsync(staged, ct);
            if (headers.Count == 0) throw new InvalidOperationException("CSV header row not found or empty.");
        }
        catch (Exception ex)
        {
            staged.Status = ProcessingStatus.Failed;
            staged.ErrorCode = ProcessingErrorCode.SchemaRegistrationFailed;
            staged.ErrorMessage = ex.Message;
            await _stagedRepo.UpdateAsync(staged, ct);
            throw;
        }

        try
        {
            var schema = new DataTableSchema
            {
                TableName = _names.Generate(staged.Id, staged.OriginalFileName),
                OriginalFileName = staged.OriginalFileName,
                Columns = _cols.Build(headers)
            };

            await _schemaRepo.AddAsync(schema, ct);

            staged.SchemaId = schema.Id;
            staged.Stage = ProcessingStage.SchemaRegistered;
            staged.Status = ProcessingStatus.InProgress;
            staged.ErrorCode = ProcessingErrorCode.None;
            staged.ErrorMessage = null;

            await _stagedRepo.UpdateAsync(staged, ct);
            return (schema, staged);
        }
        catch (Exception ex)
        {
            staged.Status = ProcessingStatus.Failed;
            staged.ErrorCode = ProcessingErrorCode.SchemaDbWriteFailed;
            staged.ErrorMessage = ex.Message;
            await _stagedRepo.UpdateAsync(staged, ct);
            throw;
        }
    }
}