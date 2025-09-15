using Application.WorkFlow.Abstractions;
using Domain.Entities;
using Infrastructure.DbConfig.Abstraction;

namespace Infrastructure.Workflows;

public class WorkflowWriter : IWorkflowWriter
{
    private readonly IEtlDbContextFactory _contextFactory;

    public WorkflowWriter(IEtlDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task AddAsync(Workflow workflow, CancellationToken ct)
    {
        var ctx = _contextFactory.CreateWorkflowDbContext();
        ctx.Workflows.Add(workflow);
        await ctx.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Workflow workflow, CancellationToken ct)
    {
        var ctx = _contextFactory.CreateWorkflowDbContext();
        ctx.Workflows.Update(workflow);
        await ctx.SaveChangesAsync(ct);
    }
}