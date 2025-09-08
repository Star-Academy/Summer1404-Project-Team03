using MediatR;

namespace Application.Tables.Commands;

public record RenameTableCommand(
    int SchemaId,
    string NewTableName
) : IRequest;
