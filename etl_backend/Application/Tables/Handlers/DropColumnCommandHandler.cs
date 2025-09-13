using Application.Common.Exceptions;
using Application.Services.Repositories.Abstractions;
using Application.Tables.Commands;
using MediatR;

namespace Application.Tables.Handlers;

public class DropColumnsCommandHandler : IRequestHandler<DropColumnsCommand>
{
    private readonly IDataTableSchemaRepository _schemaRepo;
    private readonly IDataTableColumnRepository _columnRepo;
    private readonly IColumnRepository _physicalColumnRepo;

    public DropColumnsCommandHandler(
        IDataTableSchemaRepository schemaRepo,
        IDataTableColumnRepository columnRepo,
        IColumnRepository physicalColumnRepo)
    {
        _schemaRepo = schemaRepo;
        _columnRepo = columnRepo;
        _physicalColumnRepo = physicalColumnRepo;
    }

    public async Task Handle(DropColumnsCommand request, CancellationToken ct)
    {
        if (request.ColumnIds == null || request.ColumnIds.Count == 0)
            return;

        var schema = await _schemaRepo.GetByIdWithColumnsAsync(request.SchemaId, ct)
                     ?? throw new NotFoundException("Schema", request.SchemaId);

        var existingIds = schema.Columns.Select(c => c.Id).ToHashSet();
        var notFound = request.ColumnIds.Where(id => !existingIds.Contains(id)).ToList();
        if (notFound.Count > 0)
            throw new UnprocessableEntityException("Some columns were not found: " + string.Join(", ", notFound));

        var remaining = schema.Columns.Count - request.ColumnIds.Count;
        if (remaining < 1)
            throw new ConflictException("Cannot drop all columns. A table must have at least one column.");

        var namesToDrop = schema.Columns
            .Where(c => request.ColumnIds.Contains(c.Id))
            .Select(c => c.ColumnName)
            .ToList();

        if (!await _physicalColumnRepo.TableExistsAsync("public", schema.TableName, ct))
            throw new ConflictException("Physical table does not exist.");

        try
        {
            await _physicalColumnRepo.DropColumnsAsync("public", schema.TableName, namesToDrop, ct);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to drop columns in database.", ex);
        }
        try
        {
            await _columnRepo.DeleteByIdsAsync(request.ColumnIds, ct);
        }
        catch (Exception ex)
        {
            throw new ApplicationException(
                $"Columns were dropped physically but metadata update failed. Schema ID: {request.SchemaId}. " +
                $"Manual intervention required to sync metadata.",
                ex);
        }
    }
}