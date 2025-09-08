using Application.Common.Exceptions;
using Application.Repositories.Abstractions;
using Application.Tables.Queries;
using Application.ValueObjects;
using MediatR;

namespace Application.Tables.Handlers;

public class GetTableDetailsQueryHandler : IRequestHandler<GetTableDetailsQuery, TableDetailsDto>
{
    private readonly IDataTableSchemaRepository _schemaRepo;
    private readonly ITableRepository _tableRepo;

    public GetTableDetailsQueryHandler(
        IDataTableSchemaRepository schemaRepo,
        ITableRepository tableRepo)
    {
        _schemaRepo = schemaRepo;
        _tableRepo = tableRepo;
    }

    public async Task<TableDetailsDto> Handle(GetTableDetailsQuery request, CancellationToken ct)
    {
        var schema = await _schemaRepo.GetByIdWithColumnsAsync(request.SchemaId, ct)
                     ?? throw new NotFoundException("Schema", request.SchemaId);

        var exists = await _tableRepo.TableExistsAsync("public", schema.TableName, ct);
        var approxRowCount = exists ? await _tableRepo.GetApproximateRowCountAsync("public", schema.TableName, ct) : 0;
        var sizeBytes = exists ? await _tableRepo.GetTotalSizeAsync("public", schema.TableName, ct) : 0;

        return new TableDetailsDto
        {
            SchemaId = schema.Id,
            TableName = schema.TableName,
            PhysicalExists = exists,
            RowCountApprox = approxRowCount,
            SizeBytes = sizeBytes,
            Columns = schema.Columns
                .OrderBy(c => c.OrdinalPosition)
                .Select(c => new ColumnDetailsDto
                {
                    Ordinal = c.OrdinalPosition,
                    Name = c.ColumnName,
                    Type = c.ColumnType
                })
                .ToList()
        };
    }
}