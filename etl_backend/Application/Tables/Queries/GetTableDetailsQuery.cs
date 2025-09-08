using Application.ValueObjects;
using MediatR;

namespace Application.Tables.Queries;

public record GetTableDetailsQuery(int SchemaId) : IRequest<TableDetailsDto>;