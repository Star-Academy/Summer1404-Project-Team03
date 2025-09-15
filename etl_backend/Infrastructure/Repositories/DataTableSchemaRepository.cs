using Application.Services.Repositories.Abstractions;
using Domain.Entities;
using Infrastructure.DbConfig.Abstraction;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories;

public sealed class DataTableSchemaRepository : IDataTableSchemaRepository
{
    private readonly IEtlDbContextFactory _ctxFactory;

    public DataTableSchemaRepository(IEtlDbContextFactory ctxFactory)
    {
        _ctxFactory = ctxFactory ?? throw new ArgumentNullException(nameof(ctxFactory));
    }

    public async Task<List<DataTableSchema>> ListAsync(CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        return await ctx.DataTableSchemas
                        .AsNoTracking()
                        .ToListAsync(ct);
    }

    public async Task<DataTableSchema?> GetByIdWithColumnsAsync(int id, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        return await ctx.DataTableSchemas
                        .Include(s => s.Columns) 
                        .FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task AddAsync(DataTableSchema schema, CancellationToken ct = default)
    {
        if (schema == null) throw new ArgumentNullException(nameof(schema));
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        ctx.DataTableSchemas.Add(schema);
        await ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(DataTableSchema schema, CancellationToken ct = default)
    {
        if (schema == null) throw new ArgumentNullException(nameof(schema));
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        ctx.DataTableSchemas.Update(schema);
        await ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateTableNameAsync(int id, string newTableName, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(newTableName))
            throw new ArgumentException("New table name cannot be empty.", nameof(newTableName));

        await using var ctx = _ctxFactory.CreateSchemaDbContext();

        var entity = await ctx.DataTableSchemas.FirstOrDefaultAsync(s => s.Id == id, ct);
        if (entity == null) return; 

        entity.TableName = newTableName;
        await ctx.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateSchemaDbContext();

        var entity = await ctx.DataTableSchemas.FindAsync(new object?[] { id }, ct);
        if (entity == null) return; 

        ctx.DataTableSchemas.Remove(entity);
        await ctx.SaveChangesAsync(ct);
    }
}
