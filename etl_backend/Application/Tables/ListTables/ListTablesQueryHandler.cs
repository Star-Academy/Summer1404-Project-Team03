using Application.Abstractions;
using Application.Tables.ListTables.ServiceAbstractions;
using Application.Tables.Queries;
using MediatR;

namespace Application.Tables.Handlers;

public class ListTablesQueryHandler : IRequestHandler<ListTablesQuery, List<TableListItem>>
{
    private readonly ITablesListService _svc;

    public ListTablesQueryHandler(ITablesListService svc)
    {
        _svc = svc;
    }

    public async Task<List<TableListItem>> Handle(ListTablesQuery request, CancellationToken ct)
    {
        var data = await _svc.ListAsync(ct);

        return data
            .OrderByDescending(s => s.Id)
            .Select(s => new TableListItem(
                s.Id,
                s.TableName,
                s.OriginalFileName ?? "",
                s.Columns?.Count ?? 0
            ))
            .ToList();
    }
}