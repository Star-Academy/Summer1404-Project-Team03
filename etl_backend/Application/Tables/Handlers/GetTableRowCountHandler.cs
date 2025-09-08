using Application.Common.Exceptions;
using Application.Repositories.Abstractions;
using Application.Tables.Queries;
using Application.ValueObjects;
using MediatR;

namespace Application.Tables.Handlers;

public class GetTableRowCountQueryHandler : IRequestHandler<GetTableRowCountQuery, RowCountDto>
{
    private readonly IDataTableSchemaRepository _schemaRepo;
    private readonly ITableRepository _tableRepo;

    public GetTableRowCountQueryHandler(
        IDataTableSchemaRepository schemaRepo,
        ITableRepository tableRepo)
    {
        _schemaRepo = schemaRepo;
        _tableRepo = tableRepo;
    }

    public async Task<RowCountDto> Handle(GetTableRowCountQuery request, CancellationToken ct)
    {
        var schema = await _schemaRepo.GetByIdWithColumnsAsync(request.SchemaId, ct)
                     ?? throw new NotFoundException("Schema", request.SchemaId);

        var exists = await _tableRepo.TableExistsAsync("public", schema.TableName, ct);
        if (!exists)
            throw new ConflictException("Physical table does not exist.");

        if (!request.Exact)
        {
            var approx = await _tableRepo.GetApproximateRowCountAsync("public", schema.TableName, ct);
            return new RowCountDto { Exact = false, Count = approx };
        }

        var exact = await _tableRepo.GetExactRowCountAsync("public", schema.TableName, ct);
        return new RowCountDto { Exact = true, Count = exact };
    }
}