using MediatR;

namespace Application.Tables.Commands;

public record DropColumnsCommand(
    int SchemaId,
    List<int> ColumnIds
) : IRequest;