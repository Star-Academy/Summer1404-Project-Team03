using etl_backend.DbConfig.Abstraction;
using etl_backend.Domain.Entities;
using etl_backend.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

public sealed class DataTableSchemaRepository : IDataTableSchemaRepository
{
    private readonly IEtlDbContextFactory _ctxFactory;

    public DataTableSchemaRepository(IEtlDbContextFactory ctxFactory)
    {
        _ctxFactory = ctxFactory;
    }

    public async Task<List<DataTableSchema>> ListAsync(CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        var ef = ctx as DbContext ?? throw new InvalidOperationException("Concrete DbContext is required.");

        return await ef.Set<DataTableSchema>()
            .Include(s => s.Columns)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<DataTableSchema?> GetByIdWithColumnsAsync(int id, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        var ef = ctx as DbContext ?? throw new InvalidOperationException("Concrete DbContext is required.");

        return await ef.Set<DataTableSchema>()
            .Include(s => s.Columns)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task AddAsync(DataTableSchema schema, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        var ef = ctx as DbContext ?? throw new InvalidOperationException("Concrete DbContext is required.");

        await ef.Set<DataTableSchema>().AddAsync(schema, ct);
        await ef.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(DataTableSchema schema, CancellationToken ct = default)
    {
        // your existing implementation that replaces columns in a transaction
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        var ef = ctx as DbContext ?? throw new InvalidOperationException("Concrete DbContext is required.");

        await using var tx = await ef.Database.BeginTransactionAsync(ct);

        var tracked = await ef.Set<DataTableSchema>()
            .Include(s => s.Columns)
            .FirstOrDefaultAsync(s => s.Id == schema.Id, ct)
            ?? throw new InvalidOperationException($"Schema {schema.Id} not found.");

        tracked.OriginalFileName = schema.OriginalFileName ?? tracked.OriginalFileName;

        if (tracked.Columns.Count > 0)
        {
            ef.Set<DataTableColumn>().RemoveRange(tracked.Columns);
            await ef.SaveChangesAsync(ct);
        }

        foreach (var col in schema.Columns.OrderBy(c => c.OrdinalPosition))
        {
            col.Id = 0;
            col.DataTableSchemaId = tracked.Id;
        }
        await ef.Set<DataTableColumn>().AddRangeAsync(schema.Columns, ct);

        await ef.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);
    }

    public async Task UpdateTableNameAsync(int id, string newTableName, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        var ef = ctx as DbContext ?? throw new InvalidOperationException("Concrete DbContext is required.");

        var stub = new DataTableSchema { Id = id };
        ef.Attach(stub);
        stub.TableName = newTableName.Trim();
        ef.Entry(stub).Property(x => x.TableName).IsModified = true;

        await ef.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        var ef = ctx as DbContext ?? throw new InvalidOperationException("Concrete DbContext is required.");

        var exists = await ef.Set<DataTableSchema>()
            .AsNoTracking()
            .AnyAsync(s => s.Id == id, ct);

        if (!exists)
            throw new InvalidOperationException($"Schema {id} not found.");

        var stub = new DataTableSchema { Id = id };
        ef.Attach(stub);
        ef.Remove(stub);

        await ef.SaveChangesAsync(ct);
    }
}
