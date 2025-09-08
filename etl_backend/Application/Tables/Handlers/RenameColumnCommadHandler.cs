using Application.Abstractions;
using Application.Common.Exceptions;
using Application.Repositories;
using Application.Repositories.Abstractions;
using Application.Tables.Commands;
using MediatR;

namespace Application.Tables.Handlers;

public class RenameColumnCommandHandler : IRequestHandler<RenameColumnCommand>
{
    private readonly IDataTableSchemaRepository _schemaRepo;
    private readonly IDataTableColumnRepository _columnRepo;
    private readonly IColumnRepository _physicalColumnRepo;
    private readonly IColumnNameSanitizer _sanitizer;

    public RenameColumnCommandHandler(
        IDataTableSchemaRepository schemaRepo,
        IDataTableColumnRepository columnRepo,
        IColumnRepository physicalColumnRepo,
        IColumnNameSanitizer sanitizer)
    {
        _schemaRepo = schemaRepo;
        _columnRepo = columnRepo;
        _physicalColumnRepo = physicalColumnRepo;
        _sanitizer = sanitizer;
    }

    public async Task Handle(RenameColumnCommand request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.NewName))
            throw new UnprocessableEntityException("New column name is required.");

        var schema = await _schemaRepo.GetByIdWithColumnsAsync(request.SchemaId, ct)
                     ?? throw new NotFoundException("Schema", request.SchemaId);

        var column = schema.Columns.FirstOrDefault(c => c.Id == request.ColumnId)
                    ?? throw new NotFoundException("Column", request.ColumnId);
        var maxLen = 63; 
        var sanitized = _sanitizer.Sanitize(request.NewName.Trim(), maxLen);
        if (string.IsNullOrWhiteSpace(sanitized))
            throw new UnprocessableEntityException("Sanitized name is empty.");
        if (schema.Columns.Any(c => c.Id != request.ColumnId &&
                                    string.Equals(c.ColumnName, sanitized, StringComparison.OrdinalIgnoreCase)))
            throw new UnprocessableEntityException($"A column with name '{sanitized}' already exists.");
        if (!await _physicalColumnRepo.TableExistsAsync("public", schema.TableName, ct))
            throw new ConflictException("Physical table does not exist.");

        var oldName = column.ColumnName;

        try
        {
            await _physicalColumnRepo.RenameColumnAsync("public", schema.TableName, oldName, sanitized, ct);
            await _columnRepo.UpdateNameAsync(request.ColumnId, sanitized, ct);
        }
        catch
        {
            try
            {
                await _physicalColumnRepo.RenameColumnAsync("public", schema.TableName, sanitized, oldName, ct);
            }
            catch { /* ignored */ }
            throw;
        }
    }
}