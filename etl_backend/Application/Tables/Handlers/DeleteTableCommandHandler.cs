using Application.Abstractions;
using Application.Common.Exceptions;
using Application.Tables.Commands;
using MediatR;

namespace Application.Tables.Handlers;

public class DeleteTableCommandHandler : IRequestHandler<DeleteTableCommand>
{
    private readonly ITableManagementService _svc;

    public DeleteTableCommandHandler(ITableManagementService svc)
    {
        _svc = svc;
    }

    public async Task Handle(DeleteTableCommand request, CancellationToken ct)
    {
        try
        {
            await _svc.DeleteAsync(request.SchemaId, ct);
        }
        catch (InvalidOperationException ex)
        {
            throw new NotFoundException("Table", request.SchemaId);
        }
    }
}