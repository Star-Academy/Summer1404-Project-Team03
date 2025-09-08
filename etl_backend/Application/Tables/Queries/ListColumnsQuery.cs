using MediatR;

namespace Application.Tables.Queries;

public record ListColumnsQuery(int SchemaId) : IRequest<List<ColumnListItem>>;

public record ColumnListItem(
    int Id,
    int OrdinalPosition,
    string Name,
    string Type,
    string? OriginalName
);