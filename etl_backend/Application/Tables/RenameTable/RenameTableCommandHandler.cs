using Application.Abstractions;
using Application.Common.Exceptions;
using Application.Tables.Commands;
using Application.Tables.RenameTable.ServiceAbstractions;
using MediatR;

namespace Application.Tables.Handlers;

public class RenameTableCommandHandler : IRequestHandler<RenameTableCommand>
{
    private readonly ITableRenameService _svc;

    public RenameTableCommandHandler(ITableRenameService svc)
    {
        _svc = svc;
    }

    public async Task Handle(RenameTableCommand request, CancellationToken ct)
    {
        try
        {
            await _svc.RenameAsync(request.SchemaId, request.NewTableName, ct);
        }
        catch (ArgumentException ex)
        {
            throw new UnprocessableEntityException(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            throw new NotFoundException("Table", request.SchemaId);
        }
    }
}