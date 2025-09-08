using MediatR;

namespace Application.Tables.Queries;

public record ListTablesQuery : IRequest<List<TableListItem>>;

public record TableListItem(
    int SchemaId,
    string TableName,
    string OriginalFileName,
    int ColumnCount
);