using Application.Common.Exceptions;
using Application.Tables.Commands;
using Application.Tables.DeleteTable.ServiceAbstractions;
using MediatR;

namespace Application.Tables.DeleteTable;

public class DeleteTableCommandHandler : IRequestHandler<DeleteTableCommand>
{
    private readonly ITableDeleteService _svc;

    public DeleteTableCommandHandler(ITableDeleteService svc)
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