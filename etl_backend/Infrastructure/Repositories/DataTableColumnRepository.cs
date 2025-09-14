using Application.Services.Repositories.Abstractions;
using Domain.Entities;
using Infrastructure.DbConfig.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class DataTableColumnRepository : IDataTableColumnRepository
{
    private readonly IEtlDbContextFactory _ctxFactory;

    public DataTableColumnRepository(IEtlDbContextFactory ctxFactory)
        => _ctxFactory = ctxFactory ?? throw new ArgumentNullException(nameof(ctxFactory));

    public async Task UpdateNameAsync(int id, string newName, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("New name cannot be empty.", nameof(newName));

        await using var ctx = _ctxFactory.CreateSchemaDbContext();

        var updated = await ctx.DataTableColumns
                               .Where(x => x.Id == id)
                               .ExecuteUpdateAsync(s => s.SetProperty(p => p.ColumnName, newName), ct);
        if (updated == 0) throw new KeyNotFoundException($"Column {id} not found.");
    }

    public async Task DeleteByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default)
    {
        var list = ids?.Distinct().ToList() ?? new List<int>();
        if (list.Count == 0) return;

        await using var ctx = _ctxFactory.CreateSchemaDbContext();

        await ctx.DataTableColumns
                 .Where(x => list.Contains(x.Id))
                 .ExecuteDeleteAsync(ct);
    }
}

