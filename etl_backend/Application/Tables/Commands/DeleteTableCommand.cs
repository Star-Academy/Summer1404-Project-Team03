using MediatR;

namespace Application.Tables.Commands;

public record DeleteTableCommand(int SchemaId) : IRequest;