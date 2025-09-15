using Application.WorkFlow.Abstractions;
using Domain.Entities;
using Infrastructure.DbConfig.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Workflows;

public class WorkflowReader : IWorkflowReader
{
    private readonly IEtlDbContextFactory _contextFactory;

    public WorkflowReader(IEtlDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Workflow?> GetByIdAsync(string id, string userId, CancellationToken ct)
    {
        await using var ctx = _contextFactory.CreateWorkflowDbContext();

        var workflow = await ctx.Workflows
            .Include(w => w.Plugins.OrderBy(p => p.Order))
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId, ct);
        return workflow;
    }

    public async Task<List<Workflow>> ListByUserAsync(string userId, CancellationToken ct)
    {
        var ctx = _contextFactory.CreateWorkflowDbContext();
        return await ctx.Workflows
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync(ct);
    }
}