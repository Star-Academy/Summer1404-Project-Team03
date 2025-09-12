using Application.Abstractions;
using Application.Repositories.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Files.Abstractions;
using IColumnDefinitionBuilder = Application.Abstractions.IColumnDefinitionBuilder;

namespace Infrastructure.Files;

public sealed class SchemaRegistrationService : ISchemaRegistrationService
{
    private readonly IStagedFileRepository _stagedRepo;
    private readonly IDataTableSchemaRepository _schemaRepo;
    private readonly IHeaderProvider _headers;
    private readonly IColumnDefinitionBuilder _cols;
    private readonly ITableNameGenerator _names;
    private readonly IStagedFileStateService _state;
    private readonly IColumnTypeValidator _typeValidator;
    private readonly IColumnNameSanitizer _nameSanitizer; 

    public SchemaRegistrationService(
        IStagedFileRepository stagedRepo,
        IDataTableSchemaRepository schemaRepo,
        IHeaderProvider headers,
        IColumnDefinitionBuilder cols,
        ITableNameGenerator names,
        IStagedFileStateService state,
        IColumnTypeValidator typeValidator,
        IColumnNameSanitizer? nameSanitizer = null) 
    {
        _stagedRepo = stagedRepo;
        _schemaRepo = schemaRepo;
        _headers = headers;
        _cols = cols;
        _names = names;
        _state = state;
        _typeValidator = typeValidator;
        _nameSanitizer = nameSanitizer;
    }

    public async Task<(DataTableSchema Schema, StagedFile Staged)> RegisterAsync(
        int stagedFileId,
        IReadOnlyDictionary<int, string> columnTypesByOrdinal,
        IReadOnlyDictionary<int, string> columnNamesByOrdinal,
        CancellationToken ct = default)
    {
        var staged = await _stagedRepo.GetByIdAsync(stagedFileId, ct)
                    ?? throw new InvalidOperationException($"Staged file {stagedFileId} not found.");

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

            _typeValidator.ValidateOrThrow(
                columns.Select(c =>
                {
                    var t = "string";
                    if (columnTypesByOrdinal != null)
                        columnTypesByOrdinal.TryGetValue(c.OrdinalPosition, out t);
                    return (c.OrdinalPosition, t ?? "string");
                }));

            foreach (var col in columns)
            {
                var rawType = columnTypesByOrdinal != null && columnTypesByOrdinal.TryGetValue(col.OrdinalPosition, out var t)
                    ? t
                    : "string";

                if (!_typeValidator.TryNormalize(rawType, out var norm))
                    throw new ArgumentException($"Invalid column type at ordinal {col.OrdinalPosition}: '{rawType}'");

                col.ColumnType = norm;
                if (columnNamesByOrdinal != null && columnNamesByOrdinal.TryGetValue(col.OrdinalPosition, out var newName))
                {
                    var finalName = _nameSanitizer?.Sanitize(newName, col.OrdinalPosition) ?? newName.Trim();
                    if (string.IsNullOrWhiteSpace(finalName))
                        throw new ArgumentException($"Invalid column name at ordinal {col.OrdinalPosition}: '{newName}'");

                    col.ColumnName = finalName;
                }
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