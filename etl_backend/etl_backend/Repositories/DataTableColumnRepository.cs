using etl_backend.DbConfig.Abstraction;
using etl_backend.Domain.Entities;
using etl_backend.Repositories.Abstractions;

namespace etl_backend.Repositories;

using Microsoft.EntityFrameworkCore;

public sealed class DataTableColumnRepository : IDataTableColumnRepository
{
    private readonly IEtlDbContextFactory _ctxFactory;
    public DataTableColumnRepository(IEtlDbContextFactory ctxFactory) => _ctxFactory = ctxFactory;

    public async Task AddRangeAsync(IEnumerable<DataTableColumn> columns, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        await ctx.DataTableColumns.AddRangeAsync(columns, ct);
        await ctx.SaveChangesAsync(ct);
    }

    public async Task<List<DataTableColumn>> GetBySchemaIdAsync(int schemaId, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        return await ctx.DataTableColumns
            .Where(c => c.DataTableSchemaId == schemaId) 
            .OrderBy(c => c.OrdinalPosition)
            .ToListAsync(ct);
    }

    public async Task DeleteBySchemaIdAsync(int schemaId, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        var toDelete = await ctx.DataTableColumns.Where(c => c.DataTableSchemaId == schemaId).ToListAsync(ct);
        ctx.DataTableColumns.RemoveRange(toDelete);
        await ctx.SaveChangesAsync(ct);
    }
}
