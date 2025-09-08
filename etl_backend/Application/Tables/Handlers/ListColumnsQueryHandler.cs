using Application.Common.Exceptions;
using Application.Repositories.Abstractions;
using Application.Tables.Queries;
using MediatR;

namespace Application.Tables.Handlers;

public class ListColumnsQueryHandler : IRequestHandler<ListColumnsQuery, List<ColumnListItem>>
{
    private readonly IDataTableSchemaRepository _schemaRepo;

    public ListColumnsQueryHandler(IDataTableSchemaRepository schemaRepo)
    {
        _schemaRepo = schemaRepo;
    }

    public async Task<List<ColumnListItem>> Handle(ListColumnsQuery request, CancellationToken ct)
    {
        var schema = await _schemaRepo.GetByIdWithColumnsAsync(request.SchemaId, ct)
                     ?? throw new NotFoundException("Schema", request.SchemaId);

        return schema.Columns
            .OrderBy(c => c.OrdinalPosition)
            .Select(c => new ColumnListItem(
                c.Id,
                c.OrdinalPosition,
                c.ColumnName,
                c.ColumnType,
                c.OriginalColumnName
            ))
            .ToList();
    }
}