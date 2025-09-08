using MediatR;

namespace Application.Tables.Commands;

public record RenameColumnCommand(
    int SchemaId,
    int ColumnId,
    string NewName
) : IRequest;