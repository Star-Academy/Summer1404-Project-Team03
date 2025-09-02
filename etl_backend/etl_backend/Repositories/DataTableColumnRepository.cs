using etl_backend.DbConfig.Abstraction;
using etl_backend.Domain.Entities;
using etl_backend.Repositories.Abstractions;

namespace etl_backend.Repositories;

using Microsoft.EntityFrameworkCore;

public sealed class DataTableColumnRepository : IDataTableColumnRepository
{
    private readonly IEtlDbContextFactory _ctxFactory;

    public DataTableColumnRepository(IEtlDbContextFactory ctxFactory) => _ctxFactory = ctxFactory;

    public async Task<DataTableColumn?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        var ef = (DbContext)ctx;
        return await ef.Set<DataTableColumn>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<DataTableColumn?> FindBySchemaAndNameAsync(int schemaId, string columnName, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        var ef = (DbContext)ctx;
        return await ef.Set<DataTableColumn>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.DataTableSchemaId == schemaId && x.ColumnName == columnName, ct);
    }

    public async Task<List<DataTableColumn>> ListBySchemaAsync(int schemaId, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        var ef = (DbContext)ctx;
        return await ef.Set<DataTableColumn>()
            .AsNoTracking()
            .Where(x => x.DataTableSchemaId == schemaId)
            .OrderBy(x => x.OrdinalPosition)
            .ToListAsync(ct);
    }

    public async Task UpdateNameAsync(int id, string newName, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        var ef = (DbContext)ctx;
        var stub = new DataTableColumn { Id = id, ColumnName = string.Empty };
        ef.Attach(stub);
        stub.ColumnName = newName;
        ef.Entry(stub).Property(x => x.ColumnName).IsModified = true;
        await ef.SaveChangesAsync(ct);
    }

    public async Task DeleteByIdsAsync(IEnumerable<int> ids, CancellationToken ct = default)
    {
        var list = ids?.ToList() ?? new List<int>();
        if (list.Count == 0) return;

        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        var ef = (DbContext)ctx;

        var stubs = list.Select(i => new DataTableColumn
        {
            Id = i,
            ColumnName = string.Empty 
        }).ToList();

        ef.AttachRange(stubs);
        ef.RemoveRange(stubs);
        await ef.SaveChangesAsync(ct);
    }
}

