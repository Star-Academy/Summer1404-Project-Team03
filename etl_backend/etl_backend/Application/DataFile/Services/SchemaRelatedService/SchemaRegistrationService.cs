using etl_backend.Application.DataFile.Abstraction;
using etl_backend.Application.DataFile.Services.StageFileRelated;
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
    private readonly IStagedFileStateService _state;
    private readonly IColumnTypeValidator _typeValidator;

    public SchemaRegistrationService(
        IStagedFileRepository stagedRepo,
        IDataTableSchemaRepository schemaRepo,
        IHeaderProvider headers,
        IColumnDefinitionBuilder cols,
        ITableNameGenerator names,
        IStagedFileStateService state,
        IColumnTypeValidator typeValidator)
    {
        _stagedRepo = stagedRepo;
        _schemaRepo = schemaRepo;
        _headers = headers;
        _cols = cols;
        _names = names;
        _state = state;
        _typeValidator = typeValidator;
    }

    public async Task<(DataTableSchema Schema, StagedFile Staged)> RegisterAsync(
        int stagedFileId,
        CancellationToken ct = default)
    {
        var staged = await _stagedRepo.GetByIdAsync(stagedFileId, ct)
                    ?? throw new InvalidOperationException($"Staged file {stagedFileId} not found.");

        if (staged.Status == ProcessingStatus.Failed)
            throw new InvalidOperationException("Staged file is in failed state.");

        if (staged.Stage == ProcessingStage.TableCreated || staged.Stage == ProcessingStage.Loaded)
            throw new InvalidOperationException("Columns cannot be modified after the table has been created.");

        IReadOnlyList<string> headers;
        try
        {
            headers = await _headers.GetAsync(staged, ct);
            if (headers.Count == 0) throw new InvalidOperationException("CSV header row not found or empty.");
        }
        catch (Exception ex)
        {
            await _state.FailSchemaRegistrationAsync(staged, ex.Message, ct);
            throw;
        }

        try
        {
            var columns = _cols.Build(headers);
            // default types = "string" (already set by builder), but normalize anyway
            foreach (var col in columns)
            {
                if (!_typeValidator.TryNormalize(col.ColumnType, out var norm))
                    throw new ArgumentException($"Invalid column type at ordinal {col.OrdinalPosition}: '{col.ColumnType}'");
                col.ColumnType = norm;
            }

            DataTableSchema schema;
            if (staged.SchemaId is null)
            {
                schema = new DataTableSchema
                {
                    TableName = _names.Generate(staged.Id, staged.OriginalFileName),
                    OriginalFileName = staged.OriginalFileName,
                    Columns = columns
                };

                await _schemaRepo.AddAsync(schema, ct);
                await _state.MarkSchemaRegisteredAsync(staged, schema.Id, ct);
            }
            else
            {
                schema = await _schemaRepo.GetByIdWithColumnsAsync(staged.SchemaId.Value, ct)
                         ?? throw new InvalidOperationException($"Schema {staged.SchemaId} not found.");

                // Replace columns in-place
                schema.Columns.Clear();
                foreach (var c in columns.OrderBy(c => c.OrdinalPosition))
                    schema.Columns.Add(c);

                await _schemaRepo.UpdateAsync(schema, ct);
                // Keep staged at SchemaRegistered/InProgress
            }

            return (schema, staged);
        }
        catch (Exception ex)
        {
            await _state.FailSchemaDbWriteAsync(staged, ex.Message, ct);
            throw;
        }
    }

    public async Task<(DataTableSchema Schema, StagedFile Staged)> RegisterAsync(
        int stagedFileId,
        IReadOnlyDictionary<int, string> columnTypesByOrdinal,
        CancellationToken ct = default)
    {
        var staged = await _stagedRepo.GetByIdAsync(stagedFileId, ct)
                    ?? throw new InvalidOperationException($"Staged file {stagedFileId} not found.");

        // if (staged.Status == ProcessingStatus.Failed)
        //     throw new InvalidOperationException("Staged file is in failed state.");

        if (staged.Stage == ProcessingStage.TableCreated || staged.Stage == ProcessingStage.Loaded)
            throw new InvalidOperationException("Columns cannot be modified after the table has been created.");

        IReadOnlyList<string> headers;
        try
        {
            headers = await _headers.GetAsync(staged, ct);
            if (headers.Count == 0) throw new InvalidOperationException("CSV header row not found or empty.");
        }
        catch (Exception ex)
        {
            await _state.FailSchemaRegistrationAsync(staged, ex.Message, ct);
            throw;
        }

        try
        {
            var columns = _cols.Build(headers);

            // Validate and normalize incoming types
            _typeValidator.ValidateOrThrow(
                columns.Select(c =>
                {
                    columnTypesByOrdinal.TryGetValue(c.OrdinalPosition, out var t);
                    return (c.OrdinalPosition, t ?? "string");
                }));

            foreach (var col in columns)
            {
                var t = columnTypesByOrdinal.TryGetValue(col.OrdinalPosition, out var raw)
                    ? raw
                    : "string";

                if (!_typeValidator.TryNormalize(t, out var norm))
                    throw new ArgumentException($"Invalid column type at ordinal {col.OrdinalPosition}: '{t}'");

                col.ColumnType = norm;
            }

            DataTableSchema schema;
            if (staged.SchemaId is null)
            {
                schema = new DataTableSchema
                {
                    TableName = _names.Generate(staged.Id, staged.OriginalFileName),
                    OriginalFileName = staged.OriginalFileName,
                    Columns = columns
                };

                await _schemaRepo.AddAsync(schema, ct);
                await _state.MarkSchemaRegisteredAsync(staged, schema.Id, ct);
            }
            else
            {
                schema = await _schemaRepo.GetByIdWithColumnsAsync(staged.SchemaId.Value, ct)
                         ?? throw new InvalidOperationException($"Schema {staged.SchemaId} not found.");

                schema.Columns.Clear();
                foreach (var c in columns.OrderBy(c => c.OrdinalPosition))
                    schema.Columns.Add(c);

                await _schemaRepo.UpdateAsync(schema, ct);
                await _state.MarkSchemaRegisteredAsync(staged, schema.Id, ct);
            }
            return (schema, staged);
        }
        catch (Exception ex)
        {
            await _state.FailSchemaDbWriteAsync(staged, ex.Message, ct);
            throw;
        }
    }
}
