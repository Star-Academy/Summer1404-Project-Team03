using Application.Abstractions;
using Application.Common.Exceptions;
using Application.Files.Queries;
using Application.Repositories.Abstractions;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Files.Handlers;

public class PreviewSchemaQueryHandler : IRequestHandler<PreviewSchemaQuery, ColumnPreviewResponse>
{
    private readonly IStagedFileRepository _stagedRepo;
    private readonly IHeaderProvider _headerProvider;
    private readonly IColumnDefinitionBuilder _columnDefinitionBuilder;
    private readonly IDataTableSchemaRepository _schemaRepo; // ✅ Added

    public PreviewSchemaQueryHandler(
        IStagedFileRepository stagedRepo,
        IHeaderProvider headerProvider,
        IColumnDefinitionBuilder columnDefinitionBuilder,
        IDataTableSchemaRepository schemaRepo) // ✅ Injected
    {
        _stagedRepo = stagedRepo;
        _headerProvider = headerProvider;
        _columnDefinitionBuilder = columnDefinitionBuilder;
        _schemaRepo = schemaRepo;
    }

    public async Task<ColumnPreviewResponse> Handle(PreviewSchemaQuery request, CancellationToken ct)
    {
        var staged = await _stagedRepo.GetByIdAsync(request.StagedFileId, ct);
        if (staged is null)
            throw new NotFoundException($"Staged file with ID {request.StagedFileId} not found.");

        if (staged.Status == ProcessingStatus.Failed)
            throw new ConflictException("Staged file is in failed state.");

        // ✅ Try to fetch existing schema
        DataTableSchema? existingSchema = null;
        if (staged.SchemaId.HasValue)
        {
            existingSchema = await _schemaRepo.GetByIdWithColumnsAsync(staged.SchemaId.Value, ct);
        }

        List<ColumnPreviewItem> columns;

        if (existingSchema != null && existingSchema.Columns.Any())
        {
            columns = existingSchema.Columns
                .OrderBy(c => c.OrdinalPosition)
                .Select(c => new ColumnPreviewItem(
                    c.OrdinalPosition,
                    c.ColumnName,
                    c.OriginalColumnName ?? c.ColumnName, 
                    c.ColumnType.ToString()
                ))
                .ToList();
        }
        else
        {
            var headerNames = await _headerProvider.GetAsync(staged, ct);
            if (headerNames.Count == 0)
                throw new UnprocessableEntityException("Header row not found or empty.");

            var columnEntities = _columnDefinitionBuilder.Build(headerNames);

            columns = columnEntities
                .OrderBy(c => c.OrdinalPosition)
                .Select(c => new ColumnPreviewItem(
                    c.OrdinalPosition,
                    c.ColumnName,
                    c.OriginalColumnName,
                    c.ColumnType.ToString() 
                ))
                .ToList();
        }

        return new ColumnPreviewResponse(request.StagedFileId, columns);
    }
}