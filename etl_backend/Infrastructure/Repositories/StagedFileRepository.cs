using Application.Services.Repositories.Abstractions;
using Domain.Entities;
using Infrastructure.DbConfig.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

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

    public async Task<StagedFile?> GetByIdAsync(Guid id, CancellationToken ct = default)
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
    
    public async Task<List<StagedFile>> ListAsync(CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateStagingDbContext();
        return await ctx.StagedFiles.AsNoTracking().OrderByDescending(x => x.UploadedAt).ToListAsync(ct);
    }
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        await using var ctx = _ctxFactory.CreateStagingDbContext();

        var entity = await ctx.StagedFiles.FindAsync(new object?[] { id }, ct);
        if (entity == null)
            return; // or throw — depending on your policy

        ctx.StagedFiles.Remove(entity);
        await ctx.SaveChangesAsync(ct);
    }
}
