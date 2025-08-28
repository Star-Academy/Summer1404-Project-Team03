using etl_backend.DbConfig.Abstraction;
using etl_backend.Domain.Entities;
using etl_backend.Domain.Enums;
using etl_backend.Repositories.Abstractions;

namespace etl_backend.Repositories;

using Microsoft.EntityFrameworkCore;

public sealed class StagedFileRepository : IStagedFileRepository
{
    private readonly IEtlDbContextFactory _ctxFactory;
    public StagedFileRepository(IEtlDbContextFactory ctxFactory) => _ctxFactory = ctxFactory;

    public async Task<StagedFile> AddAsync(StagedFile entity, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateStagingDbContext();
        ctx.StagedFiles.Add(entity);
        await ctx.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<StagedFile?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateStagingDbContext();
        return await ctx.StagedFiles.FindAsync(new object?[] { id }, ct);
    }

    public async Task UpdateAsync(StagedFile entity, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateStagingDbContext();
        ctx.StagedFiles.Update(entity);
        await ctx.SaveChangesAsync(ct);
    }
}
