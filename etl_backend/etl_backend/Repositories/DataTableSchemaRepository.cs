using etl_backend.DbConfig.Abstraction;
using etl_backend.Domain.Entities;
using etl_backend.Repositories.Abstractions;

namespace etl_backend.Repositories;

using Microsoft.EntityFrameworkCore;

public sealed class DataTableSchemaRepository : IDataTableSchemaRepository
{
    private readonly IEtlDbContextFactory _ctxFactory;
    public DataTableSchemaRepository(IEtlDbContextFactory ctxFactory) => _ctxFactory = ctxFactory;

    public async Task<DataTableSchema> AddAsync(DataTableSchema schema, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        ctx.DataTableSchemas.Add(schema);
        await ctx.SaveChangesAsync(ct);
        return schema;
    }

    public async Task<DataTableSchema?> GetByIdWithColumnsAsync(int id, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        return await ctx.DataTableSchemas
            .Include(s => s.Columns)
            .FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<DataTableSchema?> GetByTableNameAsync(string tableName, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        return await ctx.DataTableSchemas
            .Include(s => s.Columns)
            .FirstOrDefaultAsync(s => s.TableName == tableName, ct);
    }

    public async Task UpdateAsync(DataTableSchema schema, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateSchemaDbContext();
        ctx.DataTableSchemas.Update(schema);
        await ctx.SaveChangesAsync(ct);
    }
}
