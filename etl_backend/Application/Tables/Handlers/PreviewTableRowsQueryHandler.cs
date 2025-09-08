using Application.Common.Exceptions;
using Application.Repositories.Abstractions;
using Application.Tables.Queries;
using Application.ValueObjects;
using MediatR;

namespace Application.Tables.Handlers;

public class PreviewTableRowsQueryHandler : IRequestHandler<PreviewTableRowsQuery, RowPreviewDto>
{
    private readonly IDataTableSchemaRepository _schemaRepo;
    private readonly ITableRepository _tableRepo;

    public PreviewTableRowsQueryHandler(
        IDataTableSchemaRepository schemaRepo,
        ITableRepository tableRepo)
    {
        _schemaRepo = schemaRepo;
        _tableRepo = tableRepo;
    }

    public async Task<RowPreviewDto> Handle(PreviewTableRowsQuery request, CancellationToken ct)
    {
        var schema = await _schemaRepo.GetByIdWithColumnsAsync(request.SchemaId, ct)
                     ?? throw new NotFoundException("Schema", request.SchemaId);

        var exists = await _tableRepo.TableExistsAsync("public", schema.TableName, ct);
        if (!exists)
            throw new ConflictException("Physical table does not exist.");

        // Clamp limits
        var limit = Math.Clamp(request.Limit, 1, 200);
        var offset = Math.Max(request.Offset, 0);

        return await _tableRepo.PreviewRowsAsync(
            "public",
            schema.TableName,
            schema.Columns.OrderBy(c => c.OrdinalPosition).ToList(),
            offset,
            limit,
            request.OrderBy,
            request.Direction,
            ct
        );
    }
}