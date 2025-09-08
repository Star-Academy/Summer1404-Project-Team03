using Application.ValueObjects;
using MediatR;

namespace Application.Tables.Queries;

public record GetTableRowCountQuery(int SchemaId, bool Exact = false) : IRequest<RowCountDto>;