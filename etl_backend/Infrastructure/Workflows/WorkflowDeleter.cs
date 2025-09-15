using Application.WorkFlow.Abstractions;
using Infrastructure.DbConfig.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Workflows;

public class WorkflowDeleter : IWorkflowDeleter
{
    private readonly IEtlDbContextFactory _contextFactory;

    public WorkflowDeleter(IEtlDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task DeleteAsync(string id, string userId, CancellationToken ct)
    {
        var ctx = _contextFactory.CreateWorkflowDbContext();
        var workflow = await ctx.Workflows
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId, ct);
        if (workflow != null)
        {
            ctx.Workflows.Remove(workflow);
            await ctx.SaveChangesAsync(ct);
        }
    }
}